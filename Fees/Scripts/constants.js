var constants = {
    serviceUrl: "Inventory",
    metadata: {
        "Product:#Sample.Models": {
            key: ["ID"],
            fields: {
                ID: { type: "String:#System" },
                Manufacturer: { type: "String:#System" },
                Name: { type: "String:#System" },
                Price: { type: "Decimal:#System" },
                ProductDescription: { type: "String:#System" },
                Rating: { type: "Int32:#System" }
            },
            rules: {
                ID: { required: true, maxlength: 128 },
                Name: { required: true },
                Price: { range: [0, 1000000] },
                Rating: { range: [0, 5] }
            }
        },
        "TestProduct:#Sample.Models": {
            key: ["ID"],
            fields: {
                ErrorScenario: { type: "String:#System" },
                ID: { type: "Int32:#System" },
                Manufacturer: { type: "String:#System" },
                Name: { type: "String:#System" },
                Price: { type: "Decimal:#System" },
                Rating: { type: "Int32:#System" }
            },
            rules: {
                ID: { required: true },
                Name: { required: true },
                Price: { range: [0, 1000000] },
                Rating: { range: [0, 5] }
            }
        }
    }
};
// Honor global settings for testing different dataprovider.
if (parent.scenarioConstants) {
    constants = parent.scenarioConstants;
    constants.provider = upshot.riaDataProvider;
}

var meta = constants.metadata ? upshot.metadata(constants.metadata) : null;