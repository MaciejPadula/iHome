function getRoomCard(id, name, description, image, devices) {
    let roomCard = document.createElement('div');
    roomCard.className = "card room-card";
    roomCard.dataset.roomid = id;
    roomCard.addEventListener("drop", (ev) => {
        ev.preventDefault();
        const deviceId = ev.dataTransfer.getData("deviceId");
        const roomId = ev.target.dataset.roomid;
        $.ajax({
            contentType: "application/json",
            data: JSON.stringify({
                "deviceId": deviceId,
                "roomId": parseInt(roomId)
            }),
            processData: true,
            type: 'POST',
            url: '/api/Rooms/SetDeviceRoom/'
        }).done((data) => {
            loadRooms();
        });
    });
    roomCard.addEventListener("dragover", (ev) => {
        ev.preventDefault();
    });



    content = "";
    if (image != "") {
        let img = document.createElement("img");
        img.className = "card-img-top";
        img.src = image;
        img.dataset.roomid = id;
        roomCard.append(img);
    }
    let roomBody = document.createElement("div");
    roomBody.className = "card-body";
    roomBody.dataset.roomid = id;

    //ROOM BODY CONTROLS
    let roomTitle = document.createElement("h5");
    roomTitle.className = "card-title";
    roomTitle.dataset.roomid = id;
    roomTitle.innerHTML = name;

    let roomDescription = document.createElement("p");
    roomDescription.className = "card-text";
    roomDescription.dataset.roomid = id;
    roomDescription.innerHTML = description;

    let removeRoomButton = document.createElement("input");
    removeRoomButton.type = "button";
    removeRoomButton.className = "btn btn-primary remove-button";
    removeRoomButton.dataset.roomid = id;
    removeRoomButton.value = "Remove";
    removeRoomButton.addEventListener("click", removeRoom);

    //content += "<input type=\"button\" class=\"\" value=\"Remove\" onClick=\"removeRoom(" + id + ")\"/>";

    let deviceList = document.createElement("div");
    deviceList.className = "room-devices";
    deviceList.dataset.roomid = id;

    devices.forEach(device => {
        deviceList.append(getDeviceCard(device));
    });


    roomBody.append(roomTitle);
    roomBody.append(roomDescription);
    roomBody.append(deviceList);
    roomBody.append(removeRoomButton);
    roomCard.append(roomBody);
    return roomCard;
    //"<div class=\"card room-card\" data-roomid=\"" + id + "\" id=\"" + id + "\" ondrop=\"drop(event)\" ondragover=\"allowDrop(event)\">" + content + "</div>";
}


function getDeviceCard(device) {
    let deviceCard = document.createElement("div");
    deviceCard.className = "card device-card";
    deviceCard.draggable = true;
    deviceCard.dataset.deviceid = device.deviceId;
    deviceCard.addEventListener("dragend", (ev) => {
        ev.target.style.display = "inline-block";
    });
    deviceCard.addEventListener("dragstart", (ev) => {
        ev.dataTransfer.effectAllowed = "copyMove";
        ev.dataTransfer.setData("deviceId", ev.target.dataset.deviceid);
        //ev.target.style.display = "none";
        setTimeout(() => ev.target.style.display = "none", 0);
    });
    deviceCard.addEventListener("dragenter", (ev) => {
        ev.dataTransfer.dropEffect = "move";
    });

    let renameDeviceButton = document.createElement("button");
    renameDeviceButton.className = "btn btn-sm rounded-0";
    renameDeviceButton.style.position = "absolute";
    renameDeviceButton.style.left = "0";
    renameDeviceButton.innerHTML = (getEditIcon());
    renameDeviceButton.setAttribute("data-bs-toggle", "modal");
    renameDeviceButton.setAttribute("data-bs-target", "#renameDeviceModal");
    //data-bs-toggle="modal" data-bs-target="#addRoomModal"

    let deviceBody = document.createElement("div");
    deviceBody.className = "card-body device-body";

    let deviceTitle = document.createElement("h5");
    deviceTitle.className = "card-title";
    deviceTitle.innerHTML = device.deviceName;


    let deviceImage = document.createElement("img");
    deviceImage.className = "device-image";
    deviceImage.src = getDeviceImageUrl(device);
    deviceImage.draggable = false;

    
    deviceBody.append(deviceTitle);
    deviceBody.append(deviceControls(device));

    deviceCard.append(renameDeviceButton);
    deviceCard.append(deviceImage);
    deviceCard.append(deviceBody);
    return deviceCard;
}

function deviceControls(device) {
    let dataForm = document.createElement("form");
    dataForm.id = device.deviceId;
    dataForm.dataset.deviceid = device.deviceId;

    const data = JSON.parse(device.deviceData);
    if (device.deviceType == 1) {
        let colorInput = document.createElement("input");
        colorInput.name = "color";
        colorInput.type = "color";
        colorInput.dataset.deviceid = device.deviceId;
        colorInput.value = rgbToHex(data.Red, data.Green, data.Blue);
        colorInput.addEventListener("change", saveRgbLamp);

        let stateBody = document.createElement("div");
        stateBody.className = "form-check form-switch";

        let stateButton = document.createElement("input");
        stateButton.id = "id1";
        stateButton.name = "state";
        stateButton.type = "checkbox";
        stateButton.dataset.deviceid = device.deviceId;
        stateButton.className = "form-check-input";
        stateButton.role = "switch";
        if (data.State == 1) {
            stateButton.checked = true;
        }
        stateButton.addEventListener("change", saveRgbLamp);

        let label = document.createElement("label");
        label.className = "label";
        label.for = "id1";
        label.innerHTML = "Device State:";

        stateBody.append(stateButton);
        stateBody.append(label);

        dataForm.append(colorInput);
        dataForm.append(stateBody);
    }
    else if (device.deviceType == 2) {
        let temperatureRead = document.createElement("div");
        temperatureRead.className = "temperature";
        temperatureRead.style.width = "100%";
        temperatureRead.style.textAlign = "left";
        temperatureRead.innerHTML = "Temperature: 0&#8451;";

        let pressureRead = document.createElement("div");
        pressureRead.className = "pressure";
        pressureRead.style.width = "100%";
        pressureRead.style.textAlign = "left";
        pressureRead.innerHTML = "Pressure: 0 hPa";

        
        dataForm.append(temperatureRead);
        dataForm.append(pressureRead);
        setInterval(() => {
            getData(device.deviceId, (data) => {
                //console.log(JSON.parse(data).celsius)
                temperatureRead.innerHTML = "Temperature: " + JSON.parse(data).celsius + "&#8451;";
                pressureRead.innerHTML = "Pressure: " + JSON.parse(data).pressure + " hPa";
            });
        },2500);
    }

    return dataForm;
}

function getDeviceImageUrl(device) {
    if (device.deviceType == 1) {
        return "../resources/images/rgbLamp.png";
    }
    else if (device.deviceType == 2) {
        return "../resources/images/temperature.png";
    }
    else if (device.deviceType == 3) {
        return "../resources/images/pir.png";
    }

    return "";
}
function getEditIcon() {
    let svg = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-pencil-square\" viewBox=\"0 0 16 16\">";
    svg += "<path d=\"M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z\"/>";
    svg += "<path fill-rule=\"evenodd\" d=\"M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z\"/>";
    svg += "</svg>";
    return svg;
}