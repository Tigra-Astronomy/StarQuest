
$(document)
    .ready(function() {
        $.ajax({
            type: "GET",
            url: "api/cumulativescore/5762ef5c-0d1a-46ed-9af0-304a3747c3dc",
            dataType: "json",
            crossDomain: false,
            success: function(webData) {
                var chart = c3.generate({
                    bindto: "#chart",
                    data: {
                        json: webData,
                        x: "dates",
                        y: "scores"
                    },
                    axis: {
                        x: {
                            type: "timeseries",
                            tick: {
                                format: "%Y-%m-%d"
                            }
                        }
                    }
                });
            }
        });
    });