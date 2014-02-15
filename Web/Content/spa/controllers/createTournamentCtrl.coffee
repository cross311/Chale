createTournamentCtrl = ($scope, $http, Tournament) ->
    $scope.newTournament =
        Name: ''
        Description: ''
    
    $scope.create = () ->
        form = $scope.createTournament
        return if form.$invalid
        Tournament.create $scope.tournamentsVm.createTournamentHref, $scope.newTournament, () ->
            getTournaments()
            $scope.newTournament.Name = ''
            $scope.newTournament.Description = ''
    getTournaments()
    $scope
        
@chale.controller 'CreateTournamentCtrl', ['$scope','Tournament', createTournamentCtrl]
