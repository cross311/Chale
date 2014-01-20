# CoffeeScript
@chale = angular.module 'chale', ['ngRoute']

@chale.config ['$routeProvider', 
    ($routeProvider) ->
        $routeProvider.when '/tournaments',
            templateUrl: '/content/spa/templates/tournaments.html'
            controller: 'TournamentsCtrl'
        .otherwise
            redirectTo: '/tournaments'
]