import cv2
import time
import socket
import keyboard
import HandTrackingModule as htm
width = 640
height = 480

cap = cv2.VideoCapture(0)
cap.set(3, width)
cap.set(4, height)
detector = htm.HandDetector(maxHands=1,detectionCon=0.7)
socket_server = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
Socketaddressandport = ("127.0.0.1", 5052)
def capturing():

    success, img = cap.read()
    data = []
    hands,img = detector.findHands(img)
    if hands:
        hand = hands[0]
        lmlist = hand['lmList']
        print(lmlist)
        for lm in lmlist:
            data.extend([lm[0], height-lm[1], lm[2]])
        print(data)
        socket_server.sendto(str.encode(str(data)), Socketaddressandport)

    cv2.imshow("Image", img)
    cv2.waitKey(1)

 
while True:  
    try:  
        capturing()
        if keyboard.is_pressed('q'):  
            print('Exiting...')
            break  
    except:
        break