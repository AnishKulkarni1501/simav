from fastapi import FastAPI, File, UploadFile
from ultralytics import YOLO
from PIL import Image
import io
import time

app = FastAPI()

# Load the trained model once when the server starts
model = YOLO("models/best.pt")


@app.get("/")
def home():
    return {
        "status": "running",
        "message": "YOLO Detection Server"
    }


@app.post("/detect")
async def detect(file: UploadFile = File(...)):

    start_time = time.time()

    # Read uploaded image
    image_bytes = await file.read()

    # Convert bytes to PIL image
    image = Image.open(
        io.BytesIO(image_bytes)
    ).convert("RGB")

    # Run YOLO
    results = model(image, verbose=False)

    detections = []

    for result in results:

        for box in result.boxes:

            class_id = int(box.cls[0])
            confidence = float(box.conf[0])

            x1, y1, x2, y2 = box.xyxy[0].tolist()

            detections.append({
                "class": model.names[class_id],
                "confidence": confidence,
                "x1": x1,
                "y1": y1,
                "x2": x2,
                "y2": y2
            })

    inference_time = time.time() - start_time

    return {
        "detections": detections,
        "inference_time_ms": round(inference_time * 1000, 2)
    }