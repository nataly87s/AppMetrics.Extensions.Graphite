﻿// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using App.Metrics.Extensions.Reporting.Graphite.Client;
using App.Metrics.Formatting.Graphite;
using App.Metrics.Tagging;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Reporting.Graphite.Facts.Client
{
    public class GraphitePayloadTests
    {
        [Fact]
        public void can_format_payload()
        {
            var textWriter = new StringWriter();
            var payload = new GraphitePayload();
            var fieldsOne = new Dictionary<string, object> { { "key", "value" } };
            var timestampOne = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var pointOne = new GraphitePoint("measurement", fieldsOne, MetricTags.Empty, timestampOne);

            var fieldsTwo = new Dictionary<string, object>
                            {
                                { "field1key", "field1value" },
                                { "field2key", 2 },
                                { "field3key", false }
                            };
            var timestampTwo = new DateTime(2017, 1, 2, 1, 1, 1, DateTimeKind.Utc);
            var pointTwo = new GraphitePoint("measurement", fieldsTwo, MetricTags.Empty, timestampTwo);

            payload.Add(pointOne);
            payload.Add(pointTwo);

            payload.Format(textWriter);

            textWriter.ToString().Should()
                      .Be(
                          "measurement.key value 1483232461\nmeasurement.field1key field1value 1483318861\nmeasurement.field2key 2 1483318861\nmeasurement.field3key f 1483318861\n");
        }

        [Fact]
        public void when_null_point_ignore_and_dont_throw()
        {
            var payload = new GraphitePayload();

            Action action = () => { payload.Add(null); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void when_null_text_writer_ignore_and_dont_throw()
        {
            var payload = new GraphitePayload();
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var pointOne = new GraphitePoint("measurement", fields, MetricTags.Empty);

            Action action = () =>
            {
                payload.Add(pointOne);
                payload.Format(null);
            };

            action.ShouldNotThrow();
        }
    }
}