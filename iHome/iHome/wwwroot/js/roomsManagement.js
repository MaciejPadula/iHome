$(document).ready(() => {

});
function addRoom(name, description, image, uuid) {
    const dane =
    {
        "name": name,
        "description": description,
        "image": image,
        "uuid": uuid
    }
    
    $.ajax({
        contentType: "application/json",
        data: JSON.stringify(dane),
        dataType: 'json',
        processData: true,
        type: 'POST',
        url: '/api/rooms/addroom'
    }).done((data) => {
        loadRooms(uuid);
    });
    

}
function loadRooms(uuid) {
    $("#roomsContainer").html("");
    $("#waiting").css("display", "flex");
    $.get("/api/rooms/getrooms/" + uuid, (data) => {
        $("#roomsContainer").html("");
        for (var i in data) {
            $("#roomsContainer").append(getRoomCard(data[i].id, data[i].name, data[i].description, data[i].image, uuid));
        }
        $("#waiting").css("display", "none");
    });
    
}
function removeRoom(id, uuid) {
    $.post(
    "/api/rooms/removeroom/" + id, (data) => {
        loadRooms(uuid);
    });
}
function getRoomCard(id, name, description, image, uuid) {
    let content = "";
    content += "<img src=\"" + image + "\" class=\"card-img-top\" alt=\"...\">";
    content += "<div class=\"card-body\">";
    content += "<h5 class=\"card-title\">" + name + "</h5>";
    content += "<p class=\"card-text\">" + description + "</p>";
    content += "<input type=\"button\" class=\"btn btn-primary\" value=\"Remove\" onClick=\"removeRoom("+id+",'"+uuid+"')\"/>";
    content += "</div>";
    return "<div class=\"card roomCard\">" + content + "</div>";
}