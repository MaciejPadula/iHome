function getRoomCard(id, name, description, image, devices) {
    let roomCard = document.createElement('div');
    roomCard.className = "card room-card";
    roomCard.dataset.roomid = id;
    roomCard.addEventListener("drop", (ev) => {
        ev.preventDefault();
        const deviceId = ev.dataTransfer.getData("deviceId");
        const roomId = ev.currentTarget.dataset.roomid;
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
    if (image != "null" && image!="" && image!=undefined) {
        let img = document.createElement("img");
        img.className = "card-img-top";
        img.src = image;
        roomCard.append(img);
    }
    let roomBody = document.createElement("div");
    roomBody.className = "card-body";
    roomBody.dataset.roomid = id;

    //ROOM BODY CONTROLS
    let roomTitle = document.createElement("h5");
    roomTitle.className = "card-title";
    roomTitle.innerHTML = name;

    let roomDescription = document.createElement("p");
    roomDescription.className = "card-text";
    roomDescription.innerHTML = description;

    let removeRoomButton = document.createElement("button");
    removeRoomButton.className = "btn remove-button rounded-0";
    removeRoomButton.dataset.roomid = id;
    removeRoomButton.innerHTML = getRemoveIcon();
    removeRoomButton.addEventListener("click", removeRoom);

    //content += "<input type=\"button\" class=\"\" value=\"Remove\" onClick=\"removeRoom(" + id + ")\"/>";

    let deviceList = document.createElement("div");
    deviceList.className = "room-devices";

    devices.forEach(device => {
        deviceList.append(getDeviceCard(device));
    });
    deviceList.append(getNewDeviceButton());


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
    renameDeviceButton.addEventListener("click", () => {
        $("#deviceIdToRename").val(device.deviceId);
        $("#deviceNameToRename").val(device.deviceName);
    });
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

function getNewDeviceButton() {
    let card = document.createElement("div");
    card.className = "card device-card";

    let cardBody = document.createElement("div");
    cardBody.className = "card-body device-body";

    let icon = document.createElement("div");
    icon.className = "new-device-button-icon";
    icon.innerHTML = getCreateIcon();

    cardBody.append(icon);
    card.append(cardBody);

    card.addEventListener("click", (ev) => {
        alert("Do some stuff");
    });
    return card;
}

function getEditIcon() {
    let svg = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-pencil-square\" viewBox=\"0 0 16 16\">";
    svg += "<path d=\"M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z\"/>";
    svg += "<path fill-rule=\"evenodd\" d=\"M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z\"/>";
    svg += "</svg>";
    return svg;
}

function getRemoveIcon() {
    let svg = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-trash\" viewBox=\"0 0 16 16\">";
    svg += "<path d=\"M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z\"/>";
    svg += "<path fill-rule=\"evenodd\" d=\"M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z\"/>";
    svg += "</svg>";
    return svg;
}

function getCreateIcon() {
    let svg = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-plus-square-dotted\" viewBox=\"0 0 16 16\">";
    svg +="<path d=\"M2.5 0c-.166 0-.33.016-.487.048l.194.98A1.51 1.51 0 0 1 2.5 1h.458V0H2.5zm2.292 0h-.917v1h.917V0zm1.833 0h-.917v1h.917V0zm1.833 0h-.916v1h.916V0zm1.834 0h-.917v1h.917V0zm1.833 0h-.917v1h.917V0zM13.5 0h-.458v1h.458c.1 0 .199.01.293.029l.194-.981A2.51 2.51 0 0 0 13.5 0zm2.079 1.11a2.511 2.511 0 0 0-.69-.689l-.556.831c.164.11.305.251.415.415l.83-.556zM1.11.421a2.511 2.511 0 0 0-.689.69l.831.556c.11-.164.251-.305.415-.415L1.11.422zM16 2.5c0-.166-.016-.33-.048-.487l-.98.194c.018.094.028.192.028.293v.458h1V2.5zM.048 2.013A2.51 2.51 0 0 0 0 2.5v.458h1V2.5c0-.1.01-.199.029-.293l-.981-.194zM0 3.875v.917h1v-.917H0zm16 .917v-.917h-1v.917h1zM0 5.708v.917h1v-.917H0zm16 .917v-.917h-1v.917h1zM0 7.542v.916h1v-.916H0zm15 .916h1v-.916h-1v.916zM0 9.375v.917h1v-.917H0zm16 .917v-.917h-1v.917h1zm-16 .916v.917h1v-.917H0zm16 .917v-.917h-1v.917h1zm-16 .917v.458c0 .166.016.33.048.487l.98-.194A1.51 1.51 0 0 1 1 13.5v-.458H0zm16 .458v-.458h-1v.458c0 .1-.01.199-.029.293l.981.194c.032-.158.048-.32.048-.487zM.421 14.89c.183.272.417.506.69.689l.556-.831a1.51 1.51 0 0 1-.415-.415l-.83.556zm14.469.689c.272-.183.506-.417.689-.69l-.831-.556c-.11.164-.251.305-.415.415l.556.83zm-12.877.373c.158.032.32.048.487.048h.458v-1H2.5c-.1 0-.199-.01-.293-.029l-.194.981zM13.5 16c.166 0 .33-.016.487-.048l-.194-.98A1.51 1.51 0 0 1 13.5 15h-.458v1h.458zm-9.625 0h.917v-1h-.917v1zm1.833 0h.917v-1h-.917v1zm1.834-1v1h.916v-1h-.916zm1.833 1h.917v-1h-.917v1zm1.833 0h.917v-1h-.917v1zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z\"/>"
    svg +="</svg>"
    return svg;
}