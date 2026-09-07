from ultralytics import YOLO

# Load trained model
model = YOLO("models/best.pt")

# Run detection
results = model("test_images/captured_frame.jpg")

# Print detections
for result in results:

    for box in result.boxes:

        class_id = int(box.cls[0])
        confidence = float(box.conf[0])

        class_name = model.names[class_id]

        x1, y1, x2, y2 = box.xyxy[0].tolist()

        print("--------------------------------")
        print("Class:", class_name)
        print("Confidence:", round(confidence, 3))
        print("Bounding Box:")
        print("x1:", round(x1, 2))
        print("y1:", round(y1, 2))
        print("x2:", round(x2, 2))
        print("y2:", round(y2, 2))