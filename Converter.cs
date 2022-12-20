
using ProjectEarth_Data_Editor.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectEarth_Data_Editor
{
    public static class Converter
    {
        public static BuildPlate SharedToNormal(SharedBuildPlate _bp)
        {
            SharedBuildPlateData bpd = _bp.result.buildplateData;

            BuildPlate buildPlate = new BuildPlate();
            buildPlate.blocksPerMeter = bpd.blocksPerMeter;
            buildPlate.dimension = bpd.dimension;
            buildPlate.eTag = "dsasdasda";
            buildPlate.id = Guid.NewGuid();
            buildPlate.isModified = false;
            buildPlate.lastUpdated = "2022-04-13T12:42:24Z";
            buildPlate.locked = false;
            buildPlate.model = bpd.model;
            buildPlate.numberOfBlocks = 0;
            buildPlate.offset = bpd.offset;
            buildPlate.order = bpd.order;
            buildPlate.requiredLevel = 0;
            buildPlate.surfaceOrientation = bpd.surfaceOrientation;
            buildPlate.templateId = "00000000-0000-0000-0000-000000000000";
            buildPlate.type = bpd.type;
            return buildPlate;
        }
    }
}
