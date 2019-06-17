﻿using System;
using System.IO;
using Itinero;
using Itinero.IO.Osm;
using Itinero.LocalGeo;
using Itinero.Osm.Vehicles;

namespace DirectedSequenceSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var routerDb = new RouterDb();
            using (var stream = File.OpenRead("wechel.osm.pbf"))
            {
                routerDb.LoadOsmData(stream, Vehicle.Car);
            }
            
            // make sure there is a directed contraction hierarchy.
            routerDb.AddContracted(routerDb.GetSupportedProfile("car"), true);
            
            // define test locations.
            var locations = new []
            {
                new Coordinate(51.270453873703080f, 4.8008108139038080f),
                new Coordinate(51.264197451065370f, 4.8017120361328125f),
                new Coordinate(51.267446600889850f, 4.7830009460449220f),
                new Coordinate(51.260733228426076f, 4.7796106338500980f),
                new Coordinate(51.256489871317920f, 4.7884941101074220f),
                new Coordinate(4.7884941101074220f, 51.256489871317920f), // non-resolvable location.
                new Coordinate(51.270964016530680f, 4.7894811630249020f),
                new Coordinate(51.26216325894976f, 4.779932498931885f),
                new Coordinate(51.26579184564325f, 4.777781367301941f),
                new Coordinate(4.779181480407715f, 51.26855085674035f), // non-resolvable location.
                new Coordinate(51.26855085674035f, 4.779181480407715f),
                new Coordinate(51.26906437701784f, 4.7879791259765625f),
                new Coordinate(51.259820134021695f, 4.7985148429870605f),
                new Coordinate(51.257040455587656f, 4.780147075653076f),
                new Coordinate(51.299771179035815f, 4.7829365730285645f), // non-routable location.
                new Coordinate(51.256248149311716f, 4.788386821746826f),
                new Coordinate(51.270054481615624f, 4.799646735191345f),
                new Coordinate(51.252984777835955f, 4.776681661605835f)
            };
            
            var router = new Router(routerDb);
            var sequence = router.TryCalculate(routerDb.GetSupportedProfile("car"),
                locations, turnPenalty: 60, preferredTurns: null);
            var geojson = sequence.Value.ToGeoJson();
        }
    }
}