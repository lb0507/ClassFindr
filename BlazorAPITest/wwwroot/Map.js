/*
    Javascript file for implementing Leaflet mapping software    

    Version 10.27.24
*/

// Create a map that overlooks SHSU by default
let map = L.map('map').setView({ lon: -95.5474, lat: 30.7143 }, 15);

// Loads the map
export function load_map(buildingList) {

    // Add the actual map tile layer
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png',
        {
            maxZoom: 19,
            attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        }
    ).addTo(map);

    return "";
}

// Adds a marker to the map
export function add_marker(lat, long, name, desc, img) {

    var marker = L.marker([lat, long])
        .addTo(map)
        .bindPopup("<h5>" + name + "</h5>")
        .on('click', () => { update_info(name, desc, img); });

}

// Updates the information box by the map
function update_info(name, desc, img) {

    // Gets the associated information box to render the mark building info in
    var parent = document.getElementById("markerInfo");

    // Reset the information box
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }

    // Add the title to the information box
    var title = document.createElement("h2");
    title.innerHTML = "<text>" + name + "</text>";
    title.style = "border-bottom: 3px solid black";
    parent.appendChild(title);

    // Main body content container
    var contentContainer = document.createElement("div");
    contentContainer.style = "width: 100%; height: 80vh; overflow: hidden auto;";

    // Add image container
    var imgContainer = document.createElement("div");
    imgContainer.style = "width: 100%; text-align: center; margin: 20px;";

    // Add the image to the image container, add it to the main content container
    var image = document.createElement("img");
    image.src = img;
    image.alt = name;
    image.style = "height: 375px;";
    imgContainer.appendChild(image);
    contentContainer.appendChild(imgContainer);

    // Add the description to the information box
    var body = document.createElement("p");
    body.innerHTML = desc;
    contentContainer.appendChild(body);

    parent.appendChild(contentContainer);
    
}