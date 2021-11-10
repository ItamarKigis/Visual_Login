import cv2
import numpy
import sys

#X = sys.argv[2]
#Y = sys.argv[3]
#WIDTH = sys.argv[4]
#HEIGHT = sys.argv[5]

X = 200
Y = 200
WIDTH = 100
HEIGHT = 100

numpy.set_printoptions(threshold=sys.maxsize)

# Read the original image
img = cv2.imread("C:\\room_ser.jpg")
img = cv2.resize(img, (670,465), fx=0.4, fy=0.4)

# Convert to graycsale
img_gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
# Blur the image for better edge detection
img_blur = cv2.GaussianBlur(img_gray, (3,3), 0)
# Canny Edge Detection
edges = cv2.Canny(image=img_blur, threshold1=100, threshold2=200) # Canny Edge Detection


# Display Canny Edge Detection Image
cv2.imshow('Canny Edge Detection', edges)
cv2.waitKey(0)

#
BitArrayORows = edges[int(float(Y)):int(float(Y) + float(HEIGHT))]
imageBitArray = []
for BitArray in BitArrayORows:
    imageBitArray.append(BitArray[int(float(X)):int(float(X) + float(WIDTH))])


#for row in imageBitArray:

cv2.destroyAllWindows()
