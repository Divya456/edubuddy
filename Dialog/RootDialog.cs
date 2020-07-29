// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.AI.QnA.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Configuration;
using QnABot;
using Microsoft.BotBuilderSamples;
using System;

namespace Microsoft.BotBuilderSamples.Dialog
{
    /// <summary>
    /// This is an example root dialog. Replace this with your applications.
    /// </summary>
    public class RootDialog : ComponentDialog
    {
        /// <summary>
        /// QnA Maker initial dialog
        /// </summary>
        private const string InitialDialog = "initial-dialog";
        private IConfiguration configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="RootDialog"/> class.
        /// </summary>
        /// <param name="services">Bot Services.</param>
        public RootDialog(IBotServices services, IConfiguration configuration)
            : base("root")
        {
            AddDialog(new QnAMakerBaseDialog(services));

            AddDialog(new WaterfallDialog(InitialDialog)
               .AddStep(InitialStepAsync));

            // The initial child Dialog to run.
            InitialDialogId = InitialDialog;
            this.configuration = configuration;

        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var query = stepContext.Context.Activity.Text;
            string searchType = query.Substring(0, 2);
            var bingSearch = new BingSearch(stepContext.Context);
            string subscriptionKey = "9898d5f83c2d4b0cbbba7aa993680038";
            try
            {
                var subsKey = configuration.GetValue<string>("BingSearchKey");
            }
            catch(Exception ex)
            {
                throw new Exception("bing key could not be retrieved");
            }
            
            switch (searchType)
            {
                case "i ":
                    await bingSearch.ImageSearch(subscriptionKey, query.Substring(2));
                    return await stepContext.EndDialogAsync();
                case "v ":
                    await bingSearch.VideoSearch(subscriptionKey, query.Substring(2));
                    return await stepContext.EndDialogAsync();
                    
                case "w ":
                    await bingSearch.WebSearch(subscriptionKey, query.Substring(2));
                    return await stepContext.EndDialogAsync();
                default:
                    return await stepContext.BeginDialogAsync(nameof(QnAMakerDialog), null, cancellationToken);

            }
        }
    }
}
