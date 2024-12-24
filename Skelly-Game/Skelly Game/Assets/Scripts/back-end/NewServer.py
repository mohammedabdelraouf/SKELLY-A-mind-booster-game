import pickle
import socket
import time
import numpy as np
from tensorflow.keras.models import load_model
def start_server():
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind(('localhost', 8080))
    server_socket.listen(1)
    print("Server is listening on port 8080...")
    model = load_model("sub7_model.h5")
    print("server running")
    scene = 'Level1'
    indexer_path = 'signals.txt'
    while True:
        client_socket, addr = server_socket.accept()
        print(f"Connection from {addr} has been established!")
        data = client_socket.recv(1024).decode('utf-8')

        if data == "predict":
            s = time.time()
            with open(indexer_path, 'r') as file:
                signalID = [int(line.strip()) for line in file]

            print(signalID)

            with open(f'{scene}/{signalID[-1]}.pkl', 'rb') as f:
                signals_data = pickle.load(f)

            prediction = model.predict(np.expand_dims(signals_data, axis=1))

            predicted_labels = np.argmax(prediction, axis=1)
            # with open("back-end/prediction.txt", "w") as file:
            #     file.write(predicted_labels[-1])
            e = time.time()
            print((e - s) * 1000)
            print(f"{predicted_labels[-1]}")
            movement = {
                0: 'left',
                1: 'right',
                2: 'down',
                3: 'up'
            }.get(predicted_labels[-1], 'none')
            client_socket.send(movement.encode('utf-8'))
        elif data.__contains__("level") or data.__contains__("menu"):
            if data == "Main_menu":
                indexer_path = "Main_menu/Signals.txt"
            else:
                indexer_path = 'signals.txt'
            scene = data
            message = "Scene changed To :" + scene
            client_socket.send(message.encode('utf-8'))
        else:
            message = "Unknown request 404"
            client_socket.send(message.encode('utf-8'))
        client_socket.close()


if __name__ == "__main__":
    start_server()

