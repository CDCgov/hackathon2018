function generate(url) {
    var endpoint = "https://data.cdc.gov/data.json"
    var pieces = url.split("/");
    var identifier = "https://data.cdc.gov/d/" + pieces[pieces.length - 1];
    var data;
    console.log(identifier);

    var req = new XMLHttpRequest();

    req.open("GET", endpoint);
    req.addEventListener("load", function () {
        data = JSON.parse(req.responseText);
        console.log(data);

        var output;
        data.dataset.forEach(function (d) {
            if (d.landingPage == identifier) output = d;
        });

        return output;
    });
    req.send();
}

module.exports = generate;