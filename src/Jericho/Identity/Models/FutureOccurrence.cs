﻿namespace Jericho.Identity.Models
{
    using System;

    public class FutureOccurrence : Occurrence
    {
        public FutureOccurrence() : base()
        {

        }

        public FutureOccurrence(DateTime willOccurOn) : base(willOccurOn)
        {

        }
    }
}