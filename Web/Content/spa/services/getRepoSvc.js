﻿// Generated by CoffeeScript 1.6.3
(function() {
  var getRepo;

  getRepo = function($http, $log) {
    return {
      tournaments: function(callback) {
        var tournamentVm;
        tournamentVm = {
          tournaments: []
        };
        $http({
          method: 'get',
          url: '/tournaments'
        }).success(function(data, status, headers, config) {
          var t, _i, _len, _ref;
          _ref = data.Tournaments;
          for (_i = 0, _len = _ref.length; _i < _len; _i++) {
            t = _ref[_i];
            tournamentVm.tournaments.push(t);
          }
          if (angular.isFunction(callback)) {
            return callback(tournaments);
          }
        }).error(function(data, status, headers, config) {
          return $log.error({
            data: data,
            status: status,
            headers: headers,
            config: config
          });
        });
        return tournamentVm;
      }
    };
  };

  this.chale.factory('GetRepo', ['$http', '$log', getRepo]);

}).call(this);