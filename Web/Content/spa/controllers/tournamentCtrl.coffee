# CoffeeScript

tournamentCtrl = ($scope, $routeParams, $http, Tournament) ->
    tournamentId = $routeParams.tournamentId
    
    $scope.tournamentVm = Tournament.get tournamentId, (tournamentVm) ->
        $scope.tournamentVm = tournamentVm

@chale.controller 'TournamentCtrl', ['$scope', '$routeParams', '$http', 'Tournament', tournamentCtrl]