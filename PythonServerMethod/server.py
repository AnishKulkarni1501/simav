from flask import Flask, request, jsonify
import numpy as np
import cv2
from ultralytics import YOLO

app = Flask(__name__)

model = YOLO("yolov8n.pt")  # first test with this

@app.route("/detect", methods=["POST"])
def detect():
    file = request.files["image"].read()

    npimg = np.frombuffer(file, np.uint8)
    img = cv2.imdecode(npimg, cv2.IMREAD_COLOR)

    results = model(img)[0]

    detections = []

    for box in results.boxes:
        x1, y1, x2, y2 = box.xyxy[0].tolist()
        conf = float(box.conf[0])
        cls = int(box.cls[0])

        detections.append({
            "x1": x1,
            "y1": y1,
            "x2": x2,
            "y2": y2,
            "conf": conf,
            "class": cls
        })

    return jsonify(detections)

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)