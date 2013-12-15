using System;
using GameSketch;
using System.Collections.Generic;

namespace GameSketchTest
{
    internal class MockTournamentFactory
    {
        public Tournament Build()
        {
            return new Tournament("test tournament", DateTime.Now.AddDays(7), new List<Dude>(), new List<Game>(), null, 0);

        }
    }
}
