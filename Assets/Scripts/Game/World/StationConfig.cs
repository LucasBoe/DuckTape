using System.Collections.Generic;
using SS;
using UnityEngine;

public class StationConfig : ContaineableScriptableObject
{
    [SerializeField] public StationType Type;
    [TextArea] public string commaSeparatedListOfNames = "Alderbrook, Amberfield, Ashbourne, Baywick, Birch Hollow, Blackwater, Blue Harbor, Briar Glen, Bristlecone, Brookhaven, Cedarfall, Cedar Vale, Cloverford, Coldstream, Copper Cove, Cinder Ridge, Crystalford, Dawnhaven, Driftwood Bay, Eaglecrest, Elmspire, Emberfield, Fableton, Fairmeadow, Fern Hollow, Foxglove, Frost Harbor, Gildershire, Goldenbrook, Greenhaven, Hallowmere, Harborview, Hazelwick, Hearthstead, Highgate, Hillcrest, Hollowbrook, Honeybridge, Ironwood, Iverton, Juniper Ridge, Kestrel Point, Kingsvale, Lakebright, Lantern Bay, Larkspur, Larkvale, Laurelspine, Lilac Grove, Linden Falls, Little Wren, Mapleford, Marshlight, Meadowrun, Millstone, Mistwood, Moonhaven, Mossy Glen, Northwind, Oakfield, Oakhollow, Osprey Point, Pebblebrook, Pine Harbor, Pinecrest, Redfern, Riverbend, Riverhollow, Rosemead, Rowanbridge, Sable Creek, Sage Meadow, Sandbar, Seabright, Seagrass, Silverpine, Silverrun, Skylark, Snowvale, Sparrow Falls, Springtide, Starling, Steeplechase, Stonemere, Stormhaven, Sunfield, Sunhollow, Thimblewick, Thornberry, Thistlewick, Tidewater, Timberfall, Tranquil Bay, Tumblebrook, Velvet Harbor, Westering, Westmere, Whitebridge, Willowfen, Windemere";
    [SerializeField] public List<CargoConfigBase> Takes = new();
    [SerializeField] public List<CargoConfigBase> Sells = new();
    [SerializeField] private bool hasPassengers = false;
}

public enum StationType
{
    Cargo,
    Residential,
    City
}