var cgh_app = angular.module('myApp', ['ngRoute', 'ngMaterial']);

cgh_app.controller('MainCtrl', ['$scope', '$location', '$http', '$window',
    function MainCtrl($scope, $location, $http, $window) {        
        var path = "http://" + location.host + "/api/outstandings";        
        
        $scope.results_available = true;
        var flaws = false;
                
        $http.get(path)
            .success(function (data) {
                $scope.items = data;
                $scope.payload = [];

                if (data.length === 0) {
                    $scope.results_available = false;                    
                }

                for (var i = 0; i < $scope.items.length; i++) {                    
                    var temp = {
                        "ID": data[i].ID,
                        "MaterialID": data[i].MaterialID,
                        "CostCenterName": data[i].CostCenterName,
                        "IsDone": false
                    };
                    $scope.payload.push(temp);                    
                }

            })
            .error(function (error, status, headers, config) {
                alert(status);
                flaws = true;
            });

        $scope.toggle = function(index){
            $scope.payload[index].Clear = !$scope.payload[index].Clear;
        }

        $scope.submit = function ()
        {
            var packet = [];
            for (var i = 0; i < $scope.payload.length; i++)
            {
                if ($scope.payload[i].IsDone) packet.push($scope.payload[i]);
            }
           
            $http({
                method: "Post",
                url: path,
                data: JSON.stringify(packet),
                headers: { 'Content-Type': 'application/json' }
            })
                .success(function (data)
                {
                    //path = $location.absUrl() + "excel/get_recent";
                    $window.location.reload();
                })
                .error(function (error, status, headers, config)
                {
                    alert(status);
                }
                );
        };

        var messages = ["Nothing to do right now.. :)",
                        "Drink some coffee, maybe?",
                        "Let's check the wards anyway, good exercise! :D",
                        "Do the nurses know how to use the app?? :O"];

        $scope.message = messages[Math.floor(Math.random() * messages.length)];
        if (flaws) {
            $scope.message = "Trouble getting the data. Network connections are probably" +
                "down somewhere..";
        }

        $scope.convert = function (time) {
            return time.toString().replace('T', ' ');
        }
    }
]);
