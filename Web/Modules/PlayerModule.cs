using GameDataLayer;
using GameSketch;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Modules
{
    public class PlayerModule : NancyModule
    {
        
        private IRepository<Tournament> _repo;
        private TournamentService _service;

        public PlayerModule(IRepository<Tournament> repo, TournamentService service)
            : base("/tournaments/{tournamentId:int}/players/")
        {
            this._repo = repo;
            this._service = service;

            Get["/"] = List;
            Get["/create"] = _ => View["Create"];
            Post["/"] = Create;
            Get["/{id:int}"] = Display;
        }

        private dynamic List(dynamic arg)
        {
            throw new NotImplementedException();
        }

        private dynamic Create(dynamic arg)
        {
            throw new NotImplementedException();
        }

        private dynamic Display(dynamic arg)
        {
            throw new NotImplementedException();
        }
    }
}