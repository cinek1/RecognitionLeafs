import flask
from flask import request
import cv2
import os
import io
import torch
import torchvision.transforms as transforms
import numpy as np
import PIL
import torch.utils.data as data


app = flask.Flask(__name__)
app.config["DEBUG"] = True

# load model
model = torch.load("entire_model.pt")
model.eval()

loader = transforms.Compose([
    transforms.Resize(256),
    transforms.CenterCrop(256),
    transforms.ToTensor(),
    transforms.Normalize(mean=[0.485, 0.456, 0.406],
                         std=[0.229, 0.224, 0.225] )
    ])

# Image loader via file 
def image_loader(image_name):
    image = PIL.Image.open(image_name)
    image = loader(image).float()
    image = image.unsqueeze(0)
    return image 

# Classes avaible in model
classes_dict = {'Acer': 0, 'Actindia': 1, 'Ailanthus': 2, 'Alnus': 3, 'Amorpha': 4, 'Berberis': 5, 'Betula': 6, 'Buddleja': 7, 'Buxus': 8, 'Caragana': 9, 'Carpinus': 10, 'Carya': 11, 'Castanea': 12, 'Catalpa': 13, 'Ceeltis': 14, 'Cercidiphyllum': 15, 'Clemantis': 16, 'Colutea': 17, 'Cornus': 18, 'Juglans': 19, 'Kerria': 20, 'Koelreuteria': 21, 'Kolkwitzia': 22, 'Laburnunum': 23, 'Lavandula': 24, 'Ledum': 25, 'Ligustrum': 26, 'Liliodendron': 27, 'Liquidambar': 28, 'Lonicera': 29, 'Loranthus': 30, 'Lycium': 31, 'Maclura': 32, 'Mahonia': 33, 'Mespilus': 34, 'Morus': 35, 'Nerium': 36, 'Parthenocissus': 37, 'Paulownia': 38, 'Phellodendron': 39, 'Philadelphus': 40, 'Physocarpus': 41, 'Platanus': 42, 'Populus': 43, 'Prunus': 44, 'Pterocarya': 45, 'Pyracantha': 46, 'Quercus': 47, 'Rhamus': 48, 'Rhus': 49, 'Ribes': 50, 'Robinia': 51, 'Salix': 52, 'Sambucus': 53, 'Sorbaria': 54, 'Sorbus': 55, 'Sorphora': 56, 'Spiraela': 57, 'Staphylea': 58, 'Syhorimcarpos': 59, 'Syringa': 60, 'Wisteria': 61, 'Zelkova': 62}
classesss = list(classes_dict.keys()) 
UPLOAD_FOLDER = os.path.basename('uploads')

# Server Post adress and method
@app.route('/upload', methods=['POST'])
def upload():
    file = request.files['file']
    in_memory_file = io.BytesIO()
    file.save(in_memory_file)
    data = np.fromstring(in_memory_file.getvalue(), dtype=np.uint8)
    img = cv2.imdecode(data, 1)
    cv2.imwrite(file.filename, img)
    image = image_loader(file.filename)
    output = model.forward(image.cuda())
    output = torch.exp(output)
    probs, classes = output.topk(1, dim=1)
    Name = classesss[classes.item()]
    HyperLink = r"https://en.wikipedia.org/wiki/" + Name
    Probability = int(probs.item() * 100)
    return "{" + f'Name: "{Name}", HyperLink: "{HyperLink}", Probability: "{Probability}"' + "}"


app.run()