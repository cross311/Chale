# CoffeeScript

tournamentsCtrl = ($scope, $http, Tournament) ->
    $scope.newTournament =
        Name: ''
        Description: ''
    
    getTournaments = ->
        $scope.tournamentsVm = Tournament.getCollection()
    
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
        
@chale.controller 'TournamentsCtrl', ['$scope', '$http', 'Tournament', tournamentsCtrl]