using GameDataLayer;
using GameSketch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nancy.Web.Modules
{
    public class TournamentModule : NancyModule
    {
        private IRepository<Tournament> _repo;
        private TournamentService _service;

        public TournamentModule(IRepository<Tournament> repo, TournamentService service)
            : base("/tournaments")
        {
            this._repo = repo;
            this._service = service;

            Get["/"] = List;
        }

        private dynamic List(dynamic _)
        {
            return View["list"];
        }
    }
}