# CoffeeScript

tournamentCtrl = ($scope, $routeParams, $http, Tournament, Player, Game) ->
    tournamentId = $routeParams.tournamentId
    
    gamesReturned = false
    playersReturned = false
    
    $scope.gamesToBePlayed = []
    $scope.gamesPlayed = []
    games = Game.getCollection tournamentId, () ->
        angular.forEach games, (game) ->
            if game.IsOpen or game.IsInProgress then $scope.gamesToBePlayed.push proccessToBePlayedGame game
            else $scope.gamesPlayed.push processCompletedGame game
        
    $scope.players = Player.getCollection tournamentId
    
    processToBePlayedGame = (game) ->
        player1 = game.Players[0]
        player2 = if game.Players.length >= 1 then game.Players[1] else { Name: 'ToBeDetermined' } 
        game.player1 = player1.Name
        game.player2 = player2.Name
        
    processCompletedGame = (game) ->
        player1 = game.Players[0]
        player2 = game.Players[1]
        winner = if player1.Id is games.WinningPlayer.Id then player1 else player2
        loser = if winner.Id is player1.Id then player2 else player1
        game.winner = winner.Name
        game.loser = loser.Name
        
    
    $scope.tournamentVm = Tournament.get tournamentId, (tournamentVm) ->
        $scope.tournamentVm = tournamentVm
    
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

@chale.controller 'TournamentCtrl', ['$scope', '$routeParams', '$http', 'Tournament', 'Player', 'Game', tournamentCtrl]