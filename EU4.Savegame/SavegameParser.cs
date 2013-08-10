using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    /// <summary>
    /// The sole purpose of this class is to encapsulate any state that is
    /// needed while parsing the save game.
    /// </summary>
    internal class SavegameParser : IParadoxRead
    {
        /// <summary>
        /// The next province id that is expected in the stream
        /// </summary>
        private int currentParsedProvince;

        /// <summary>
        /// The string representation of the next province id that is expected
        /// in the stream.
        /// </summary>
        /// <remarks>
        /// The thought behind this is that it every time a token is analyzed,
        /// it is faster compare strings many times, rather than to try and
        /// convert the token to a number and compare that against <see
        /// cref="currentParsedProvince"/>.  There will be, at most, a couple
        /// thousands province id tokens, out of the millions of them, thus to
        /// try and convert the millions of tokens
        /// to numbers would be more inefficient than to compare millions of
        /// strings.
        /// </remarks>
        private string currentParsedProvinceStr;

        private Savegame game;
        internal SavegameParser(Savegame game)
        {
            this.game = game;
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            if (token == this.currentParsedProvinceStr && parser.CurrentIndent == 0)
            {
                var newProv = new Province(this.currentParsedProvince++);
                this.game.Provinces.Add(parser.Parse<Province>(newProv));
                this.currentParsedProvinceStr = this.currentParsedProvince.ToString();
            }
        }
    }
}
