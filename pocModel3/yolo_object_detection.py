import cv2
import numpy as np
from PIL import Image
import sys

def main():
    # Load Yolo/
    net = cv2.dnn.readNet("C:\yolov3.weights", "C:\yolov3.cfg")
    classes = []
    with open(r"C:\Users\משתמש\Documents\pocModel3\coco.names", "r") as f:
        classes = [line.strip() for line in f.readlines()]
    layer_names = net.getLayerNames()


    output_layers = [layer_names[i[0] - 1] for i in net.getUnconnectedOutLayers()]
    colors = np.random.uniform(0, 255, size=(len(classes), 3))

    # Loading image
    img = cv2.imread(sys.argv[1])
    img = cv2.resize(img, None, fx=0.4, fy=0.4)
    height, width, channels = img.shape

    # Detecting objects
    blob = cv2.dnn.blobFromImage(img, 0.00392, (416, 416), (0, 0, 0), True, crop=False)

    net.setInput(blob)
    outs = net.forward(output_layers)

    # Showing informations on the screen
    class_ids = []
    confidences = []
    boxes = []

    for out in outs:
        for detection in out:
            scores = detection[5:]
            class_id = np.argmax(scores)
            confidence = scores[class_id]
            if confidence > 0.5:
                # Object detected
                center_x = int(detection[0] * width)
                center_y = int(detection[1] * height)
                w = int(detection[2] * width)
                h = int(detection[3] * height)

                # Rectangle coordinates
                x = int(center_x - w / 2)
                y = int(center_y - h / 2)

                boxes.append([x, y, w, h])
                confidences.append(float(confidence))
                class_ids.append(class_id)

                #print("Object number {0} detection with accuracy of {1}".format(len(boxes), confidence))
                #print("Center of the object is ({0},{1}). Height is {2} and width is {3}".format(center_x, center_y, h, w))

    indexes = cv2.dnn.NMSBoxes(boxes, confidences, 0.5, 0.4)
    font = cv2.FONT_HERSHEY_PLAIN

    print(width, height)

    for i in range(len(boxes)):
        if i in indexes:
            x, y, w, h = boxes[i]
            print(x,y,w,h)
            label = str(classes[class_ids[i]])
            color = colors[class_ids[i]]
            cv2.rectangle(img, (x, y), (x + w, y + h), color, 2)
            cv2.putText(img, label, (x, y + 30), font, 3, color, 3)
            #getting the image
               #im = img[y: y + h, x: x + w]
               #cv2.imshow("Image", im)
               #cv2.waitKey(0)


if __name__ == '__main__':
    main()
