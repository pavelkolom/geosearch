
   function getLocationByIP(){
        $.ajax({
            url:'https://localhost:44394/Ip/location?ip=' + $("#search").val(),
            method:'GET',
            contentType: "application/json",    
            dataType:'json',
            success:function(response){
                var trHTML='';
                trHTML=trHTML+'<tr><td>'+
                    response.country+'</td><td>'+
                    response.region+'</td><td>'+
                    response.postal+'</td><td>'+
                    response.city+'</td><td>'+
                    response.organization+'</td><td>'+
                    response.latitude+'</td><td>'+
                    response.longitude+'</td><td>'+
                    response.range.ip_from+'</td><td>'+
                    response.range.ip_to+'</td><td>'+
                    response.range.location_index+'</td></tr>'; 
                $('#tBody').empty();
                $('#tBody').append(trHTML);
            },
            error: function() {
                alert('error');
           }
          });
       }

       
   function getLocationsByCityname(){
    $.ajax({
        url:'https://localhost:44394/City/locations?city=' + $("#search").val(),
        method:'GET',
        contentType: "application/json",    
        dataType:'json',
        success:function(response){
            var trHTML='';
            for(var i=0;i<response.length;i++){
                trHTML=trHTML+'<tr><td>'+
                    response[i].index+'</td><td>'+
                    response[i].country+'</td><td>'+
                    response[i].region+'</td><td>'+
                    response[i].postal+'</td><td>'+
                    response[i].city+'</td><td>'+
                    response[i].organization+'</td><td>'+
                    response[i].latitude+'</td><td>'+
                    response[i].longitude+'</td></tr>'; 

           }
           $('#tBody').empty();
           $('#tBody').append(trHTML);
        },
        error: function() {
            alert('error');
       }
      });
   }

   function register(){
        // Get the input field
        var input = document.getElementById("search");
        // Execute a function when the user presses a key on the keyboard
        input.addEventListener("keypress", function(event) {
            // If the user presses the "Enter" key on the keyboard
            if (event.key === "Enter") {
                // Cancel the default action, if needed
                event.preventDefault();
                // Trigger the button element with a click
                document.getElementById("srchbtn").click();
            }
        });
   }

   function keypress(event) {
    // If the user presses the "Enter" key on the keyboard
    if (event.key === "Enter") {
        // Cancel the default action, if needed
        event.preventDefault();
        // Trigger the button element with a click
        document.getElementById("srchbtn").click();
    }}
