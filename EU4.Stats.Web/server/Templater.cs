using System.IO;
using EdgeJs;
using System.Threading.Tasks;
using System;

namespace EU4.Stats.Web
{
    /// <summary>
    /// The Templater provides a thread safe mechanism for rendering objects
    /// into html, caching the template, and updating the template whenever the
    /// template changes. The current implementation uses mutual exclusive
    /// locking. The reason for this is two-fold. One, it is easy to get
    /// threading right, and secondly, generating the resulting html isn't
    /// computationally intensive so the lock will only be acquired for a few
    /// moments.
    /// </summary>
    public class Templater : ITemplate
    {
        private readonly string file;
        private Func<object, Task<object>> gen;

        private Templater(string filepath, Func<object, Task<object>> generator) 
        {
            file = filepath;
            gen = generator;
        }

        async public Task<string> Render(object obj)
        {
            return (string)(await gen(obj));
        }

        async public static Task<Templater> CreateTemplater(string filepath)
        {
            var gen = await Edge.Func(
                @"var jade = require('jade');
                  var _ = require('underscore');
                  var _s = require('underscore.string');
                  _.mixin(_s);
                  return function(file, cb) {
                      var fn = jade.compileFile(file, {
                          pretty: true, debug: true, compileDebug: true
                      });

                      cb(null, function(data, cb) {
                          cb(null, fn(_.extend(data, _)));
                      });
                  };")(filepath);

            return new Templater(filepath, (Func<object, Task<object>>)gen);
        }
    }
}
