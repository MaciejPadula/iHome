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
    deviceList.id = "room-devices";
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
        ev.target.style.display = "none";
        //setTimeout(() => ev.target.style.display = "none", 0);
    });
    deviceCard.addEventListener("dragenter", (ev) => {
        ev.dataTransfer.dropEffect = "move";
    });


    let deviceBody = document.createElement("div");
    deviceBody.className = "card-body device-body";

    let deviceTitle = document.createElement("h5");
    deviceTitle.className = "card-title";
    deviceTitle.innerHTML = device.deviceName;


    let deviceImage = document.createElement("img");
    deviceImage.className = "device-image";
    deviceImage.src = "../resources/images/rgbLamp.png";
    deviceImage.draggable = false;

    
    deviceBody.append(deviceTitle);
    deviceBody.append(deviceControls(device));

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

    return dataForm;
}