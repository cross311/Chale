# CoffeeScript

tournamentsCtrl = ($scope, $http, Get) ->
    $scope.newTournament =
        Name: ''
        Description: ''
        
    $scope.tournamentsVm = Get.tournaments()
    
    $scope.create = () ->
        form = $scope.createTournament
        $http.post($scope.tournamentsVm.createTournamentHref, $scope.newTournament)
        .success (data, status, headers, config) ->
            $scope.newTournament.Name = ''
            $scope.newTournament.Description = ''
    $scope
        
@chale.controller 'TournamentsCtrl', ['$scope', '$http', 'GetRepo', tournamentsCtrl]