function getRoomCard(id, name, description, image, devices) {
    let roomCard = new CardBuilder('room')
        .withAttribute('data-roomid', id)
        .withEventListener('drop', (ev) => {
            ev.preventDefault();
            const deviceId = ev.dataTransfer.getData('deviceId');
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
        })
        .withEventListener('dragover', (ev) => {
            ev.preventDefault();
        })
        .withTitle(name)
        .withDescription(description);
    if (image != "null" && image != "" && image != undefined) {
        roomCard.withImage(image);
    }

    let deviceList = document.createElement("div");
    deviceList.className = "room-devices";
    devices.forEach(device => {
        deviceList.append(getDeviceCard(device));
    });
    deviceList.append(getNewDeviceButton(id));

    roomCard.addToBody(deviceList);
    roomCard.addToBody(new ElementBuilder('button')
        .withClassName('btn remove-button rounded-0')
        .withAttribute('data-roomid', id)
        .withInnerHTML(getRemoveIcon())
        .withEventListener('click', removeRoom)
        .build()
    );
    return roomCard.build();
}


function getDeviceCard(device) {
    let deviceCard = new CardBuilder('device')
        .withDraggable(true)
        .withAttribute('data-deviceid', device.deviceId)
        .withEventListener('dragend', (ev) => {
            ev.target.style.display = "inline-block";
        })
        .withEventListener('dragstart', (ev) => {
            ev.dataTransfer.setData('deviceId', ev.target.dataset.deviceid);
            setTimeout(() => ev.target.style.display = 'none', 0);
        })
        .withEventListener('dragenter', (ev) => {
            ev.dataTransfer.dropEffect = 'move';
        })
        .withTitle(device.deviceName)
        .addToBody(deviceControls(device));

    let renameDeviceButton = new ElementBuilder('button')
        .withClassName('btn btn-sm rounded-0')
        .withInnerHTML(getEditIcon())
        .withEventListener('click', (ev) => {
            $('#deviceIdToRename').val(device.deviceId);
            $('#deviceNameToRename').val(device.deviceName);
        })
        .withAttribute('data-bs-toggle', 'modal')
        .withAttribute('data-bs-target', '#renameDeviceModal')
        .withStyle('position', 'absolute')
        .withStyle('left', '0')
        .build();

    deviceCard.addToCard(renameDeviceButton);
    deviceCard.addToCard(new ImageBuilder()
        .withClassName('device-image')
        .withDraggable(false)
        .withSrc(getDeviceImageUrl(device))
        .build()
    );
    return deviceCard.build();
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

function getNewDeviceButton(roomId) {
    let card = document.createElement("div");
    card.className = "card device-card new-device-card";

    let cardBody = document.createElement("div");
    cardBody.className = "card-body device-body";

    let icon = document.createElement("div");
    icon.className = "new-device-button-icon";
    icon.innerHTML = getCreateIcon();

    cardBody.append(icon);
    card.append(cardBody);
    card.setAttribute("data-bs-toggle", "modal");
    card.setAttribute("data-bs-target", "#setupDeviceModal");

    card.addEventListener("click", (ev) => {
        $('#room-id-input').val(roomId);
    });
    return card;
}

function createNewDevice(deviceInfo) {
    let main = document.createElement('div');
    main.className = 'new-device-info';

    let deviceId = document.createElement('div');
    deviceId.className = 'new-device-address';
    deviceId.innerText = deviceInfo.deviceId;

    let deviceImgContainer = document.createElement('div');
    deviceImgContainer.className = 'new-device-image-container'
    let deviceImage = document.createElement('img');
    deviceImage.src = getDeviceImageUrl(deviceInfo);
    deviceImgContainer.append(deviceImage);

    let deviceNameInput = document.createElement('input');
    deviceNameInput.type = 'text';
    deviceNameInput.style.display = 'none';
    deviceNameInput.style.transition = '1s';

    let button = document.createElement('button');
    button.addEventListener('click', (ev) => {
        if (deviceNameInput.style.display == 'none') {
            deviceNameInput.style.display = 'block';
        }
        else if (deviceNameInput.value == '') {
            deviceNameInput.style.display = 'none';
        }
        else {
            let data = {
                'deviceId': deviceInfo.deviceId,
                'deviceType': deviceInfo.deviceType,
                'deviceName': deviceNameInput.value,
                'roomId': $('#room-id-input').val()
            };
            if (deviceInfo.deviceType == 1) {
                data['deviceData'] = '{"Red":255, "Green":255, "Blue":255, "State":1}';
            }
            $.ajax({
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(data),
                type: 'POST',
                url: '/api/rooms/adddevice/' + deviceInfo.id
            }).done((data) => {
                window.location.reload(true);
            });
        }
    });

    main.append(deviceNameInput);
    main.append(deviceImgContainer);
    main.append(deviceId);
    main.append(button);
    
    return main;
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