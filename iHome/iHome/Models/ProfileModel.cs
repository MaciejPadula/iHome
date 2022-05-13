using iHome.Models.iHomeComponents;

namespace iHome.Models
{
    public class ProfileModel
    {
        public string? Name { get; set; }
        public string? EmailAddress { get; set; }
        public string? ProfileImage { get; set; }
        public string? Uuid { get; set; }
        private DatabaseModel database = new DatabaseModel("ihome.database.windows.net", "rootAdmin", "VcuraBEFKR6@3PX", "iHome");
        public ProfileModel(string? name, string? emailAddress, string? profileImage, string? uuid)
        {
            Name = name;
            EmailAddress = emailAddress;
            ProfileImage = profileImage;
            this.Uuid = uuid;
        }
        /*
         <div class=\"card\" style=\"width: 18rem;\">
  <img src=\"...\" class=\"card-img-top\" alt=\"...\">
  <div class=\"card-body\">
    <h5 class=\"card-title\">Card title</h5>
    <p class=\"card-text\">Some quick example text to build on the card title and make up the bulk of the card's content.</p>
    <a href=\"#\" class=\"btn btn-primary\">Go somewhere</a>
  </div>
</div>*/
        public string RenderRooms()
        {
            int i = 0;
            string output = "";
            
            database.GetRooms(Uuid).ForEach(room =>
            {
                output += RenderRoom(room);
            });
            return output;
        }
        private string RenderRoom(Room room)
        {
            string content = "";
            content += "<img src=\""+room.image+"\" class=\"card-img-top\" alt=\"...\">";
            content += "<div class=\"card-body\">";
            content += "<h5 class=\"card-title\">"+room.name+"</h5>";
            content += "<p class=\"card-text\">"+room.description+"</p>";
            content += "<a href=\"#\" class=\"btn btn-primary\">Go somewhere</a>";
            content += "</div>";
            return "<div class=\"card\" style=\"width: 18rem;\">" + content + "</div>";
        }
    }
}
