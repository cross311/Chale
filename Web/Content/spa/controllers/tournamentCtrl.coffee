# CoffeeScript

tournamentCtrl = ($scope, $routeParams, $http, Tournament) ->
    tournamentId = $routeParams.tournamentId
    
    $scope.tournamentVm = Tournament.get tournamentId, (tournamentVm) ->
        $scope.tournamentVm = tournamentVm
    $scope.gamesToBePlayed = [
        id: 5
        player1: 'Connor'
        player2: 'Matt'
    ,
        id: 6
        player1: 'Connor'
        player2: 'Matt'
    ,
        id: 7
        player1: 'Connor'
        player2: 'Matt'
    ,
        id: 8
        player1: 'Connor'
        player2: 'Matt'
    ]
        
    $scope.gamesPlayed = [
        id: 1
        winner: 'Connor'
        loser: 'Matt'
    ,
        id: 2
        winner: 'Connor'
        loser: 'Matt'
    ,
        id: 3
        winner: 'Connor'
        loser: 'Matt'
    ,
        id: 4
        winner: 'Connor'
        loser: 'Matt'
    ]
    
    $scope.players = [
        name: 'Connor'
        games: [
            otherPlayer:'Matt'
            status: 'loss'
        ,
            otherPlayer:'Danelle'
            status: 'won'
            
        ]
    ,
        name: 'Matt'
        games: [
            otherPlayer:'Connor'
            status: 'won'
        ,
            otherPlayer:'Danelle'
            status: 'progress'
            
        ]
    ,
        name: 'Danelle'
        games: [
            otherPlayer:'Connor'
            status: 'loss'
        ,
            otherPlayer:'Matt'
            status: 'progress'
            
        ]
    ]

@chale.controller 'TournamentCtrl', ['$scope', '$routeParams', '$http', 'Tournament', tournamentCtrl]