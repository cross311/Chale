# CoffeeScript
@chale = angular.module 'chale', ['ngRoute']

@chale.config ['$routeProvider', 
    ($routeProvider) ->
        $routeProvider.when '/tournaments',
            templateUrl: '/content/spa/templates/tournaments.html'
            controller: 'TournamentsCtrl'
        .when '/tournaments/:tournamentId',
            templateUrl: '/content/spa/templates/tournament.html'
            controller: 'TournamentCtrl'
        .otherwise
            redirectTo: '/tournaments'
]