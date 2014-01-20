# CoffeeScript

tournamentsCtrl = ($scope, $http, Get) ->
    $scope.newTournament =
        Name: ''
        Description: ''
    
    getTournaments = ->
        $scope.tournamentsVm = Get.tournaments()
    
    $scope.create = () ->
        form = $scope.createTournament
        return if form.$invalid
        $http.post($scope.tournamentsVm.createTournamentHref, $scope.newTournament)
        .success (data, status, headers, config) ->
            getTournaments()
            $scope.newTournament.Name = ''
            $scope.newTournament.Description = ''
    getTournaments()
    $scope
        
@chale.controller 'TournamentsCtrl', ['$scope', '$http', 'GetRepo', tournamentsCtrl]