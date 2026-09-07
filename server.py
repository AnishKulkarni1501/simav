from fastapi import FastAPI, File, UploadFile
from ultralytics import YOLO
from PIL import Image
import io
import time
import torch

app = FastAPI()


# ============================================================
# DEVICE
# ============================================================

if torch.cuda.is_available():
    DEVICE = 0
    DEVICE_NAME = torch.cuda.get_device_name(0)
else:
    DEVICE = "cpu"
    DEVICE_NAME = "CPU"


# ============================================================
# LOAD MODEL
# ============================================================

model = YOLO("models/yolov8l.pt")

# Explicitly move model to GPU
model.to(DEVICE)

print("========================================")
print("YOLO Detection Server")
print("========================================")
print("Device:", DEVICE_NAME)
print("CUDA available:", torch.cuda.is_available())
print("========================================")


# ============================================================
# HOME
# ============================================================

@app.get("/")
def home():

    return {
        "status": "running",
        "message": "YOLO Detection Server",
        "device": DEVICE_NAME
    }


# ============================================================
# DETECTION
# ============================================================

@app.post("/detect")
async def detect(
    file: UploadFile = File(...)
):

    start_time = time.time()

    # --------------------------------------------------------
    # Read uploaded image
    # --------------------------------------------------------

    image_bytes = await file.read()

    # --------------------------------------------------------
    # Convert to PIL image
    # --------------------------------------------------------

    image = Image.open(
        io.BytesIO(image_bytes)
    ).convert("RGB")

    # --------------------------------------------------------
    # YOLO INFERENCE
    # --------------------------------------------------------

    results = model(
        image,
        device=DEVICE,
        imgsz=320,
        conf=0.5,
        verbose=False
    )

    # --------------------------------------------------------
    # Extract detections
    # --------------------------------------------------------

    detections = []

    for result in results:

        for box in result.boxes:

            class_id = int(
                box.cls[0]
            )

            confidence = float(
                box.conf[0]
            )

            x1, y1, x2, y2 = (
                box.xyxy[0].tolist()
            )

            detections.append({

                "class":
                    model.names[class_id],

                "confidence":
                    confidence,

                "x1": x1,
                "y1": y1,
                "x2": x2,
                "y2": y2
            })

    # --------------------------------------------------------
    # Timing
    # --------------------------------------------------------

    inference_time = (
        time.time() - start_time
    )

    return {

        "detections":
            detections,

        "inference_time_ms":
            round(
                inference_time * 1000,
                2
            )
    }