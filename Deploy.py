import uuid
import sys
from pathlib import Path
import os
import shutil
from distutils.dir_util import copy_tree

projectName = sys.argv[1]
dllFilePath = sys.argv[2]
dllFileName = os.path.basename(dllFilePath)

relativePath = os.path.dirname(sys.argv[0]) + "\\" + projectName + "\\"

f = open(relativePath + "ModOutput/ModInfo.json", "r")
jsonFileData = f.read();
f.close();
if jsonFileData == "[not deployed yet]" or jsonFileData == "":
    print("Setting up json file....")
    f = open(relativePath + "ModOutput/ModInfo.json", "w")
    f.write("{")
    f.write("\n\t\"DisplayName\": \"")
    f.write(projectName)
    f.write("\",\n\t\"UniqueID\": \"")
    f.write(str(uuid.uuid4()))
    f.write("\",\n\t\"MainDLLFileName\": \"")
    f.write(dllFileName)
    f.write("\",\n")
    f.write("\t\"Author\": \"X606\",\n")
    f.write("\t\"Version\": 0,\n")
    f.write("\t\"ImageFileName\": \"DefaultImage.png\",\n")
    f.write("\t\"Description\": \"\",\n")
    f.write("\t\"ModDependencies\": [],\n")
    f.write("\t\"Tags\": []\n")
    f.write("}")
    f.close()
    print("Done setting up!")

CloneDronePathFilePath = str(Path(sys.argv[0]).parent) + "\\CloneDronePath.path"

f = open(CloneDronePathFilePath, "r")
CloneDronePath = f.read()
f.close()

modPath = CloneDronePath + "/" + projectName + "/"

if not os.path.isdir(modPath):
    os.mkdir(modPath)

copy_tree(relativePath + "ModOutput/", modPath)
shutil.copyfile(dllFilePath, modPath + dllFileName)

print("Deployed the" + projectName + " mod into the game!")