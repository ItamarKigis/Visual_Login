import cv2
import numpy
import sys

X = sys.argv[2]
Y = sys.argv[3]
WIDTH = sys.argv[4]
HEIGHT = sys.argv[5]

numpy.set_printoptions(threshold=sys.maxsize)

# Read the original image
img = cv2.imread(sys.argv[1])
img = cv2.resize(img, (670,465), fx=0.4, fy=0.4)

# Convert to graycsale
img_gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
# Blur the image for better edge detection
img_blur = cv2.GaussianBlur(img_gray, (3,3), 0)
# Canny Edge Detection
edges = cv2.Canny(image=img_blur, threshold1=100, threshold2=200) # Canny Edge Detection


# Display Canny Edge Detection Image

#
BitArrayORows = edges[int(float(Y)):int(float(Y) + float(HEIGHT))]
imageBitArray = []
for BitArray in BitArrayORows:
    imageBitArray.append(BitArray[int(float(X)):int(float(X) + float(WIDTH))])

numOfRow=0
numOfColumn=0
ListOfEdges = []
for row in imageBitArray:
    numOfColumn=0
    for number in row:
        if number == 255:
            ListOfEdges.append((numOfColumn + int(float(X)), numOfRow + int(float(Y))))
            print((numOfColumn + int(float(X)), numOfRow + int(float(Y))), end='')
            print(" ", end='')
        numOfColumn+=1
    numOfRow+=1


cv2.destroyAllWindows()
