# CoffeeScript
getRepo = ($http, $log) ->
    
    tournaments: (callback) ->
        tournamentVm = 
            tournaments: []
        $http
            method: 'get'
            url: '/tournaments'
        .success (data, status, headers, config) ->
            for t in data.Tournaments
                tournamentVm.tournaments.push t
            tournamentVm.createTournamentHref = data.CreateTournamentHref
            callback tournamentVm if angular.isFunction callback
        .error (data, status, headers, config) ->
            $log.error 
                data: data
                status: status
                headers: headers
                config: config
        tournamentVm
    
@chale.factory 'GetRepo', ['$http', '$log', getRepo]