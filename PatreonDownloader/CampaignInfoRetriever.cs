﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PuppeteerSharp;

namespace PatreonDownloader
{
    struct CampaignInfo
    {
        public string AvatarUrl { get; set; }
        public string CoverUrl { get; set; }
        public string Name { get; set; }
    }
    class CampaignInfoRetriever
    {
        private Browser _browser;

        public CampaignInfoRetriever(Browser browser)
        {
            _browser = browser;
        }

        public async Task<CampaignInfo> RetrieveCampaignInfo(long campaignId)
        {
            var page = await _browser.NewPageAsync();
            Response response = await page.GoToAsync($"https://www.patreon.com/api/campaigns/{campaignId}?include=access_rules.tier.null&fields[access_rule]=access_rule_type%2Camount_cents%2Cpost_count&fields[reward]=title%2Cid%2Camount_cents&json-api-version=1.0");
            string json = await response.TextAsync();

            CampaignAPIRoot root = JsonConvert.DeserializeObject<CampaignAPIRoot>(json);

            await page.CloseAsync();

            return new CampaignInfo
            {
                AvatarUrl = root.Data.Attributes.AvatarUrl, CoverUrl = root.Data.Attributes.CoverUrl, Name = root.Data.Attributes.Name
            };
        }
    }
}
