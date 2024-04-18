
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Microsoft.Extensions.DependencyInjection;
using StoriesOfTheLand.Data;
using System;
using System.Linq;

namespace StoriesOfTheLand.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, StoriesOfTheLandContext context)
        {

            //for testing always start with the same database.
            if (bool.Parse(Environment.GetEnvironmentVariable("BYPASS_AUTHENTICATION") ?? "true"))
            {
                context.Database.EnsureDeleted();
            }
            // Look for any movies.
            context.Database.EnsureCreated();

            // Look for any Specimens.
            if (context.Specimen.Any())
            {
                return;   // DB has been seeded
            }

            context.Resource.AddRange(

                 new Resource
                 {
                     ResourceTitle = "Saskatchewan Polytechnic",
                     ResourceDescription = "The best college",
                     ResourceURL = "https://saskpolytech.ca/",
                     ResourceImage = "images/default.png",
                     FR_Resource = new FR_Resource
                     {
                         FR_ResourceTitle = "École polytechnique de la Saskatchewan",
                         FR_ResourceDescription = "Le meilleur collège"
                     }
                 },

                 new Resource
                 {
                     ResourceTitle = "NCTR",
                     ResourceDescription = "The NCTR is a place of learning and dialogue where the truths of the residential school experience will be honoured and kept safe for future generations. ",
                     ResourceURL = "https://nctr.ca/",
                     ResourceImage = "images/default.png",

                     FR_Resource = new FR_Resource
                     {
                         FR_ResourceTitle = "NCTR",
                         FR_ResourceDescription = "Le NCTR est un lieu d’apprentissage et de dialogue où les vérités de l’expérience des pensionnats indiens seront honorées et préservées pour les générations futures.",
                     }
                 }

                );



            context.Specimen.AddRange(

                  new Specimen
                  {
                      SpecimenDescription = //Blueberry
                      @"A small shrub, 10-50cm tall, growing in sandy or gravel soils. It thrives in clearings of coniferous stands of the boreal forest. 
                             This woody plant can grow in dense clusters and is characterized by its soft, lance-shaped, velvety leaves. The spring flowers are shaped like delicate white urns,
                             which develop into the petite, blue fruit, familiar to all “pickers”!",
                      LatinName = "Vaccinium myrtilloides",
                      EnglishName = "Velvet Leaf Blueberry",
                      CreeName = "Idinimin",
                      CulturalSignificance = "When you stumble on her you may see a pretty wildflower, but she is so much more, strong, beautiful and healing in nature the lungwort plant offers relief from stomach ailments, diarrhea, wounds healing and most commonly like its name its used for coughs, colds and irritation of the lungs.",
                      MediaList = new List<Media>()
                      {
                                    new Media
                                    {
                                        SpecimenID = 1,
                                        MediaPath = "Blueberry.png",
                                        MediaType = "Image"
                                    },
                                    new Media
                                    {
                                        SpecimenID = 1,
                                        MediaPath = "blueberry2.png",
                                        MediaType = "Image"
                                    },
                                    new Media
                                    {
                                        SpecimenID = 1,
                                        MediaPath = "Blueberry.m4a",
                                        MediaType = "Audio"
                                    }

                      },
                      Latitude = 53.87971,
                      Longitude = -105.40012,
                      FR_Specimen = new FR_Specimen()
                      {
                          FR_EnglishName = "Myrtille à feuilles de velours",
                          FR_CulturalSignificance = "Lorsque vous tombez sur elle, vous verrez peut-être une jolie fleur sauvage, mais elle est bien plus encore, forte, belle et de nature curative. La plante pulmonaire offre un soulagement des maux d'estomac, de la diarrhée, de la cicatrisation des plaies et, le plus souvent, comme son nom l'indique, elle est utilisée pour la toux. rhumes et irritation des poumons.",
                          FR_SpecimenDescription = "Un petit arbuste de 10 à 50 cm de haut, poussant dans les sols sableux ou graveleux. Il prospère dans les clairières des peuplements de conifères de la forêt boréale."
                      }
                  },
                  new Specimen
                  {
                      SpecimenDescription = //Horsetail
                      @"Horsetail plants tend to favour cool, moist, forested areas. Species grow from low to the ground to 1m tall. All horsetails are characterized by jointed, grooved, 
                            hollow stems with a honeycomb like top where the spores are housed. Horsetails reproduce by spores as apposed to seed. 
                           They are ancient primitive plants dating back over 300 million years!",
                      LatinName = "Equisetum species",
                      EnglishName = "Horsetail",
                      CulturalSignificance = "When you stumble on her you may see a pretty wildflower, but she is so much more, strong, beautiful and healing in nature the lungwort plant offers relief from stomach ailments, diarrhea, wounds healing and most commonly like its name its used for coughs, colds and irritation of the lungs.",
                      MediaList = new List<Media>()
                      {
                                new Media
                                    {
                                        SpecimenID = 2,
                                        MediaPath = "Horsetail.png",
                                        MediaType = "Image"
                                    }
                      },

                      Latitude = 53.88056,
                      Longitude = -105.40177,

                      FR_Specimen = new FR_Specimen
                      {
                          FR_EnglishName = "Prêle",
                          FR_CulturalSignificance = "La prêle, souvent aperçue comme une simple mauvaise herbe, détient en réalité des propriétés fortes, belles et curatives, offrant un soulagement pour les maux d'estomac, la diarrhée et aidant à la cicatrisation des plaies. C'est également une plante couramment utilisée pour les toux, les rhumes et les irritations pulmonaires.",
                          FR_SpecimenDescription = "Les prêles préfèrent les zones forestières fraîches et humides. Elles se caractérisent par leurs tiges articulées, rainurées et creuses, avec un sommet en nid d'abeille où sont logées les spores. Les prêles se reproduisent par spores et non par graines."
                      }
                  },

                  new Specimen
                  {
                      SpecimenDescription = //Labrador Tea
                      @"Labrador tea is a low shrub found in bogs, swamps, and moist lowland woods in nutrient poor soil. This plant keeps its leaves all year round though they 
                            often turn brownish orange in the winter. The leaves alternate around the stem like a spiral staircase. The leaves are thick and leathery with orange fuzzy hairs 
                            on the underside. White coloured flowers sit on top of the plant.",
                      LatinName = "Ledum groenlandicum",
                      EnglishName = "Labrador Tea",
                      CreeName = "Maskêkopakwa",
                      CulturalSignificance = "When you stumble on her you may see a pretty wildflower, but she is so much more, strong, beautiful and healing in nature the lungwort plant offers relief from stomach ailments, diarrhea, wounds healing and most commonly like its name its used for coughs, colds and irritation of the lungs.",
                      MediaList = new List<Media>()
                      {
                                new Media
                                    {
                                        SpecimenID = 3,
                                        MediaPath = "LabradorTea.png",
                                        MediaType = "Image"
                                    },
                                new Media
                                    {
                                        SpecimenID = 3,
                                        MediaPath = "LabTeaLeaves.png",
                                        MediaType = "Image"
                                    },
                                new Media
                                {
                                        SpecimenID = 3,
                                        MediaPath = "LabTeaPlants.png",
                                        MediaType = "Image"
                                    }
                      },


                      Latitude = 53.88045,
                      Longitude = -105.4016,

                      FR_Specimen = new FR_Specimen
                      {
                          FR_EnglishName = "Thé du Labrador",
                          FR_CulturalSignificance = "Le thé du Labrador, plus qu'une simple plante, est un symbole de force, de beauté et de guérison. Il est traditionnellement utilisé pour apaiser les maux d'estomac, les diarrhées, favoriser la cicatrisation des plaies et est couramment employé contre les toux, les rhumes et les irritations pulmonaires.",
                          FR_SpecimenDescription = "Le thé du Labrador est un arbuste bas trouv&eacute; dans les tourbières, les marécages et les bois humides des terres basses sur des sols pauvres en nutriments. Cette plante conserve ses feuilles toute l'année."
                      }

                  },
              new Specimen
              {
                  SpecimenDescription = //Lungwort
                  @"Lungwort is an erect, perennial plant, (growing from 20-80cm tall) commonly found in moist woods, and meadows. 
                        It has wide pointed leaves that alternate up the stem and pink or blue bell-shaped flowers on bowing branches
                         Leaves are covered with short hairs making them feel rough to the touch. ",
                  LatinName = "Mertensia paniculata",
                  EnglishName = "Lungwort",
                  CulturalSignificance = "When you stumble on her you may see a pretty wildflower, but she is so much more, strong, beautiful and healing in nature the lungwort plant offers relief from stomach ailments, diarrhea, wounds healing and most commonly like its name its used for coughs, colds and irritation of the lungs.",
                  MediaList = new List<Media>()
                      {
                                new Media
                                    {
                                        SpecimenID = 4,
                                        MediaPath = "Lungwort.png",
                                        MediaType = "Image"
                                    }
                      },
                  Latitude = null,
                  Longitude = null,
                  //null coords
              },
              new Specimen
              {
                  SpecimenDescription = //Mint
                 @"Wild mint is found in moist soil, on shorelines, stream banks and damp clearings. It can grow from 10-60cm tall, 
                        has serrated leaves in pairs around a square stem and small, purple-pink flowers in dense whorls at the base of the leaves. Walking on or 
                        disturbing mint releases the familiar mint smell.",
                  LatinName = "Mentha arvensis",
                  EnglishName = "Wild Mint",
                  CreeName = "Amiskowihkask",
                  CulturalSignificance = "When you stumble on her you may see a pretty wildflower, but she is so much more, strong, beautiful and healing in nature the lungwort plant offers relief from stomach ailments, diarrhea, wounds healing and most commonly like its name its used for coughs, colds and irritation of the lungs.",
                  MediaList = new List<Media>()
                      {
                                new Media
                                    {
                                        SpecimenID = 5,
                                        MediaPath = "mint.png",
                                        MediaType = "Image"
                                    },
                                new Media
                                    {
                                        SpecimenID = 5,
                                        MediaPath = "Mint.m4a",
                                        MediaType = "Audio"
                                    }
                      }
                      ,
                  Latitude = 53.88059,
                  Longitude = -105.40139
              },
                  new Specimen
                  {
                      SpecimenDescription = //Mint
                     @"Wild mint is found in moist soil, on shorelines, stream banks and damp clearings. It can grow from 10-60cm tall, 
                            has serrated leaves in pairs around a square stem and small, purple-pink flowers in dense whorls at the base of the leaves. Walking on or 
                            disturbing mint releases the familiar mint smell.",
                      LatinName = "Mentha arvensis",
                      EnglishName = "Wild Mint",
                      CreeName = "Amiskowihkask",
                      CulturalSignificance = "When you stumble on her you may see a pretty wildflower, but she is so much more, strong, beautiful and healing in nature the lungwort plant offers relief from stomach ailments, diarrhea, wounds healing and most commonly like its name its used for coughs, colds and irritation of the lungs.",
                      MediaList = new List<Media>()
                      {
                                new Media
                                    {
                                        SpecimenID = 6,
                                        MediaPath = "mint.png",
                                        MediaType = "Image"
                                    },
                                new Media
                                    {
                                        SpecimenID = 6,
                                        MediaPath = "Mint.m4a",
                                        MediaType = "Audio"
                                    },
                      }
                      ,
                      Latitude = 53.87991,
                      Longitude = -105.39965

                  },
                  new Specimen
                  {
                      SpecimenDescription = //Stinging Nettle
                      @"Stinging Nettle is found in moist open areas around stream/riverbanks, open low areas, thickets, and disturbed sites. It grows tall, usually 0.5m-2.0m, with square stems and narrow, toothed leaves in pairs around the stem. Tiny inconspicuous, greenish flowers form drooping clusters at the base of the leaves. 
                            The plant spreads through underground stems called rhizomes. Note: Stinging Nettle has hairs on the leaves and stems that contain formic acid. 
                            Handling or brushing up against any part of the plant can irritate the skin, causing a burning rash that can last for days!",
                      LatinName = "Mentha arvensis",
                      EnglishName = "Stinging Nettle",
                      CreeName = "Amiskowihkask",
                      CulturalSignificance = "When you stumble on her you may see a pretty wildflower, but she is so much more, strong, beautiful and healing in nature the lungwort plant offers relief from stomach ailments, diarrhea, wounds healing and most commonly like its name its used for coughs, colds and irritation of the lungs.",
                      MediaList = new List<Media>()
                      {

                      },
                      Latitude = 53.87964,
                      Longitude = -105.39971
                  },
                  new Specimen
                  {
                      SpecimenDescription = //Paper Birch
                      @"Full grown Paper Birch trees can vary from 15-30m in height, and they are commonly found in moist, 
                            well drained, forested sites. The most recognizable feature of mature trees is the easily peelable white bark with pale or dark 
                            lenticels (horizontal, aerating structures). Bark colour can be dark or reddish brown on younger trees and branches. 
                            The leaves have a double sawtooth pattern around the edges.",
                      LatinName = "Betula papyrifera.",
                      EnglishName = "Paper Birch",
                      CreeName = "Waskway",
                      CulturalSignificance = "This tree is amongst the most used in traditional practices, from medicinal to practical day to day uses it is a great fire starter. The bark peels away in paper like sheets, can be used for birch bark biting, a beautiful but difficult practice to try. Moose calls were made, birch bark baskets for gathering plants, berries, using as water dippers. Large sheets could be used to make canoes and tarp like sheets as it is waterproof. Medicinally, the leaves are great to have in your collection for ailments ranging from urinary issues to high cholesterol and diarrhea. It is lovely pain reliever, leaves and branches can be boiled and added to bath water to help with aches, pains, and headaches. One of the most sought after and well-known gems that can be found on birch is chaga (posakan or touchwood), a black fungus with a rust-coloured center, this is a powerhouse healer, traditionally used to treat many types of cancers, great as a general everyday antioxidant tea. Every part of this tree from roots to buds is healing and energizing, a staple in cree culture.",
                      MediaList = new List<Media>()
                      {

                      },
                      Latitude = 53.8797,
                      Longitude = -105.40025
                  },
                  new Specimen
                  {
                      SpecimenDescription = //Plantain – Plantago major
                      @"An introduced species common to disturbed or cultivated areas, it is found throughout 
                            most of North America. It grows low to the ground in a cluster of strongly veined, egg-shaped leaves. 
                            Seed capsules are arranged in spikes from the center of the leaves.",
                      LatinName = "Plantago major",
                      EnglishName = "Plantain",
                      CreeName = "wāpiski-mōniyāw osit",
                      CulturalSignificance = "This plant is magical! Often found near nettle which is natures convenient way to rid the nettles nasty burn by using the leaves of plantain, you can use a spit poultice or simply rub the plant and its liquid on the affected area. Plantain is also very convenient in this area with our abundance of mosquitos as it will help take the itch away. Chewing a piece of the root and keeping it on the affected area can help draw out infection and reduce the discomfort of a toothache.",
                      MediaList = new List<Media>()
                      {

                      },
                      Latitude = 53.88073,
                      Longitude = -105.40201
                  },
                  new Specimen
                  {
                      SpecimenDescription = //Wild Rose - Rosa species
                      @"Wild Rose is found in clearings, plains, and forested areas. It is a shrub with alternate leaves that are divided into 3-7 smaller leaflets. The flowers are reddish pink with 5 petals. After flowering, the fruits or rosehips develop, which vary in shape from round to oval depending on the species. The reddish stems are covered in prickles and thorns.",
                      LatinName = "Rosa Species",
                      EnglishName = "Wild Rose",
                      CreeName = "okinīwāhtik",
                      CulturalSignificance = "This plant is full of vitamin C, it has become a habit from childhood that while walking the land you eat a few petals along the way to top up said vit c, the petals can be used in jellies, healing ointments, teas and more. The rosehips are often picked in the late fall and traditionally could be used to make necklaces. On the land we were taught trapline facemasks (crushed rosehips and bear grease) because this plant is great for the skin. Rosehips are also eaten, just be sure to spit out the seeds. The wesakechak legend says he didn’t listen to the grandmother to spit out the seeds and in the coming days he was walking around scratching his bottom, as it became irritated from the seeds, and this was the plants way of telling on him. If you ever hear an elder yelling at you about ‘itchass’, it is not Cree, they are warning you not to eat the seeds or you’ll get an itchy bottom.",
                      MediaList = new List<Media>()
                      {

                      },
                      Latitude = 53.88071,
                      Longitude = -105.40148
                  },
                  new Specimen
                  {
                      SpecimenDescription =
                      @"Willows are shrubs and trees that grow in low moist areas near or around water. They are “dioecious” meaning that there are separate male and female plants. Willows have leaves that are two to more than ten times longer than they are wide. All willow species produce tiny flowers in clusters called catkins; these soft fluffy droops are what gave rise to the name “Pussy Willow”. There are thirty-two different species of willow in Saskatchewan and differentiating species often requires extensive analysis from an experienced taxonomist.",
                      LatinName = "Salix Species",
                      EnglishName = "Red Willow",
                      CreeName = "miskwāpīmakwa",
                      CulturalSignificance = "The red willow has many uses, most common is using the bark to dye hide, and other items, this is done by boiling the bark and letting it steep with said items once cooled until the desired colour is reached, the white spruce roots can be used almost as a thread to sew, for example many birch bark baskets are stitched together by dyed roots. The medicinal properties with this plant are like willow, the bark is used in a tea or skin compress to assist in calming stomach aches, reducing inflammation, removing the itch of skin irritations like bug bites, rashes, soothing eczema, and chicken pox. This wood is also used to smoke fish.",
                      MediaList = new List<Media>()
                      {

                      },
                      Latitude = 53.88049,
                      Longitude = -105.40099
                  },
                  new Specimen
                  {
                      SpecimenDescription =
                      @"Common Yarrow is found in clearings around wooded areas, and in meadows. The leaves alternate on the stem and are divided into segments which resemble fern leaves. It has both erect stems that grow 10-70cm tall and horizontal, underground stems called rhizomes that allow it to spread over an area.",
                      LatinName = "Achillea millefolium.",
                      EnglishName = "Common Yarrow",
                      CreeName = "wāpanīwask",
                      CulturalSignificance = "This plant has many uses, the most common being the powerful ability to nourish and heal our skin, it’s a fantastic astringent. This plant can also quickly stop bleeding, relieve cramps and reduce fevers.",
                      MediaList = new List<Media>()
                      {

                      },
                      Latitude = 53.88063,
                      Longitude = -105.40099
                  },
                  new Specimen
                  {
                      SpecimenDescription = //Hair Lichen
                      @"NA",
                      LatinName = "NA.",
                      EnglishName = "Hair Lichen",
                      CreeName = "NA",
                      CulturalSignificance = "NA",
                      MediaList = new List<Media>()
                      {

                      },
                      Latitude = 53.88044,
                      Longitude = -105.4011
                  }
              );


            context.SaveChanges();

                context.Sponsor.AddRange(
                    new Sponsor
                    {
                        SponsorName = "Saskatchewan Polytechnic",
                        SponsorURL = "https://saskpolytech.ca",
                        SponsorImagePath = "Saskatchewan_Polytechnic_Logo.png",
                       
                    },
                    new Sponsor
                    {
                        SponsorName = "Saskatchewan Wildlife Federation",
                        SponsorURL = "https://swf.sk.ca",
                        SponsorImagePath = "SWF-2-Circle-Logo-horizontal-2C-copy-2048x837.png"
                    },
                    new Sponsor
                    {
                        SponsorName = "Prince Albert Grand Council",
                        SponsorURL = "https://www.pagc.sk.ca",
                        SponsorImagePath = "pa-grand-council-logo.jpg"
                    }
                    );
                context.SaveChanges();

            context.Faq.AddRange(
                new Faq
                {

                    Title = "Can anyone go into the camp",
                    Description = "Yes Everyone is free to  come in and learn the various plants"

                },
                new Faq
                {

                    Title = "Are there wild roses?",
                    Description = "Yes, there are many wild roses here around the camp"
                },
                new Faq
                {

                    Title = "Are there edible plants around here?",
                    Description = "Why yes, there are many Edible plants"
                }
                );

            context.SaveChanges();
            context.UserImage.AddRange(
                new UserImage
                {
                    IP = ":1:1:1:1:1",
                    DateUploaded = DateTime.Now,
                    status = false,
                    MediaPath = "wwwroot/media/submissions/Horsetail.png",
                },
                new UserImage
                {
                    IP = ":1:1:1:1:1",
                    DateUploaded = DateTime.Now,
                    status = false,
                    MediaPath = "wwwroot/media/submissions/LabTeaLeaves.png",
                },
                new UserImage
                {
                    IP = "169.0.0.1",
                    DateUploaded = DateTime.Now,
                    status = false,
                    MediaPath = "wwwroot/media/submissions/Lungwort.png",
                });
            context.Feedback.AddRange(
                new Feedback
                {
                    Name = "Sally Joe",
                    Email = "SallyJoe@gmail.com",
                    Subject = "Misspelled name on page",
                    SpecimenID = 1,
                    Status = Status.New,
                    CreateDate = DateTime.Now,
                    Details = "I am extremely disappointed with the product I received. It arrived late and damaged. Customer service has been unresponsive to my inquiries. This experience has left me frustrated and dissatisfied.",
                },
                 new Feedback
                 {
                     Name = "Billy Bob",
                     Email = "BillyBob@gmail.com",
                     Subject = "Specimen page is missing an image",
                     SpecimenID = 5,
                     Status = Status.New,
                     CreateDate = DateTime.Now,
                     Details = "I am writing to express my extreme dissatisfaction with the recent purchase I made from your website. Firstly, the product arrived significantly later than the promised delivery date, causing inconvenience and disruption to my plans. Upon opening the package, I was dismayed to discover that the item was damaged. The packaging seemed inadequate to protect the product during transit, as there were visible signs of mishandling and rough treatment. I immediately reached out to your customer service department to report the issue and request a resolution. However, I have been met with nothing but frustration and disappointment. Despite multiple attempts to contact your support team via phone and email, I have received no response. This lack of communication is unacceptable and reflects poorly on your company's commitment to customer satisfaction. As a loyal customer, I expected better from your brand. This experience has not only tarnished my perception of your company but has also caused me undue stress and inconvenience. I urge you to take immediate action to rectify this situation by providing a refund or replacement for the damaged product, as well as improving your customer service processes to prevent similar issues in the future. I sincerely hope that you will address my concerns promptly and restore my faith in your brand. Otherwise, I will have no choice but to take my business elsewhere and share my negative experience with others. Thank you for your attention to this matter."
                 },
                 new Feedback
                 {
                     Name = "Jonh Smith",
                     Email = "JohnSmith@gmail.com",
                     Subject = "Error on a page",
                     Status = Status.PendingReponse,
                     CreateDate = DateTime.Now,
                     Details = "I am extremely disappointed with the product I received. It arrived late and damaged. Customer service has been unresponsive to my inquiries. This experience has left me frustrated and dissatisfied.",
                 },
                  // Feedback in the last week
                  new Feedback
                  {
                      Name = "Billy Bob",
                      Email = "BillyBob@gmail.com",
                      Subject = "Specimen page is missing an image",
                      SpecimenID = 5,
                      Status = Status.New,
                      CreateDate = DateTime.Now.AddDays(-4),
                      Details = "I am writing to express my extreme dissatisfaction with the recent purchase I made from your website. Firstly, the product arrived significantly later than the promised delivery date, causing inconvenience and disruption to my plans. Upon opening the package, I was dismayed to discover that the item was damaged. The packaging seemed inadequate to protect the product during transit, as there were visible signs of mishandling and rough treatment. I immediately reached out to your customer service department to report the issue and request a resolution. However, I have been met with nothing but frustration and disappointment. Despite multiple attempts to contact your support team via phone and email, I have received no response. This lack of communication is unacceptable and reflects poorly on your company's commitment to customer satisfaction. As a loyal customer, I expected better from your brand. This experience has not only tarnished my perception of your company but has also caused me undue stress and inconvenience. I urge you to take immediate action to rectify this situation by providing a refund or replacement for the damaged product, as well as improving your customer service processes to prevent similar issues in the future. I sincerely hope that you will address my concerns promptly and restore my faith in your brand. Otherwise, I will have no choice but to take my business elsewhere and share my negative experience with others. Thank you for your attention to this matter."
                  },
                  new Feedback
                  {
                      Name = "John Smith",
                      Email = "BillyBob@gmail.com",
                      Subject = "Specimen page is missing an image",
                      SpecimenID = 5,
                      Status = Status.New,
                      CreateDate = DateTime.Now.AddDays(-4),
                      Details = "I am writing to express my extreme dissatisfaction with the recent purchase I made from your website. Firstly, the product arrived significantly later than the promised delivery date, causing inconvenience and disruption to my plans. Upon opening the package, I was dismayed to discover that the item was damaged. The packaging seemed inadequate to protect the product during transit, as there were visible signs of mishandling and rough treatment. I immediately reached out to your customer service department to report the issue and request a resolution. However, I have been met with nothing but frustration and disappointment. Despite multiple attempts to contact your support team via phone and email, I have received no response. This lack of communication is unacceptable and reflects poorly on your company's commitment to customer satisfaction. As a loyal customer, I expected better from your brand. This experience has not only tarnished my perception of your company but has also caused me undue stress and inconvenience. I urge you to take immediate action to rectify this situation by providing a refund or replacement for the damaged product, as well as improving your customer service processes to prevent similar issues in the future. I sincerely hope that you will address my concerns promptly and restore my faith in your brand. Otherwise, I will have no choice but to take my business elsewhere and share my negative experience with others. Thank you for your attention to this matter."
                  },
                  new Feedback
                  {
                      Name = "Billy Bob",
                      Email = "BillyBob@gmail.com",
                      Subject = "Specimen page is missing an image",
                      SpecimenID = 5,
                      Status = Status.New,
                      CreateDate = DateTime.Now.AddDays(-11),
                      Details = "I am writing to express my extreme dissatisfaction with the recent purchase I made from your website. Firstly, the product arrived significantly later than the promised delivery date, causing inconvenience and disruption to my plans. Upon opening the package, I was dismayed to discover that the item was damaged. The packaging seemed inadequate to protect the product during transit, as there were visible signs of mishandling and rough treatment. I immediately reached out to your customer service department to report the issue and request a resolution. However, I have been met with nothing but frustration and disappointment. Despite multiple attempts to contact your support team via phone and email, I have received no response. This lack of communication is unacceptable and reflects poorly on your company's commitment to customer satisfaction. As a loyal customer, I expected better from your brand. This experience has not only tarnished my perception of your company but has also caused me undue stress and inconvenience. I urge you to take immediate action to rectify this situation by providing a refund or replacement for the damaged product, as well as improving your customer service processes to prevent similar issues in the future. I sincerely hope that you will address my concerns promptly and restore my faith in your brand. Otherwise, I will have no choice but to take my business elsewhere and share my negative experience with others. Thank you for your attention to this matter."
                  },
                  new Feedback
                  {
                      Name = "Billy Bob",
                      Email = "BillyBob@gmail.com",
                      Subject = "Specimen page is missing an image",
                      SpecimenID = 5,
                      Status = Status.New,
                      CreateDate = DateTime.Now.AddMonths(-5),
                      Details = "I am writing to express my extreme dissatisfaction with the recent purchase I made from your website. Firstly, the product arrived significantly later than the promised delivery date, causing inconvenience and disruption to my plans. Upon opening the package, I was dismayed to discover that the item was damaged. The packaging seemed inadequate to protect the product during transit, as there were visible signs of mishandling and rough treatment. I immediately reached out to your customer service department to report the issue and request a resolution. However, I have been met with nothing but frustration and disappointment. Despite multiple attempts to contact your support team via phone and email, I have received no response. This lack of communication is unacceptable and reflects poorly on your company's commitment to customer satisfaction. As a loyal customer, I expected better from your brand. This experience has not only tarnished my perception of your company but has also caused me undue stress and inconvenience. I urge you to take immediate action to rectify this situation by providing a refund or replacement for the damaged product, as well as improving your customer service processes to prevent similar issues in the future. I sincerely hope that you will address my concerns promptly and restore my faith in your brand. Otherwise, I will have no choice but to take my business elsewhere and share my negative experience with others. Thank you for your attention to this matter."
                  },
                  new Feedback
                  {
                      Name = "Billy Bob",
                      Email = "BillyBob@gmail.com",
                      Subject = "Specimen page is missing an image",
                      SpecimenID = 5,
                      Status = Status.New,
                      CreateDate = DateTime.Now.AddMonths(-5),
                      Details = "I am writing to express my extreme dissatisfaction with the recent purchase I made from your website. Firstly, the product arrived significantly later than the promised delivery date, causing inconvenience and disruption to my plans. Upon opening the package, I was dismayed to discover that the item was damaged. The packaging seemed inadequate to protect the product during transit, as there were visible signs of mishandling and rough treatment. I immediately reached out to your customer service department to report the issue and request a resolution. However, I have been met with nothing but frustration and disappointment. Despite multiple attempts to contact your support team via phone and email, I have received no response. This lack of communication is unacceptable and reflects poorly on your company's commitment to customer satisfaction. As a loyal customer, I expected better from your brand. This experience has not only tarnished my perception of your company but has also caused me undue stress and inconvenience. I urge you to take immediate action to rectify this situation by providing a refund or replacement for the damaged product, as well as improving your customer service processes to prevent similar issues in the future. I sincerely hope that you will address my concerns promptly and restore my faith in your brand. Otherwise, I will have no choice but to take my business elsewhere and share my negative experience with others. Thank you for your attention to this matter."
                  },
                  new Feedback
                  {
                      Name = "Jonh Smith",
                      Email = "JohnSmith@gmail.com",
                      Subject = "Error on a page",
                      CreateDate = new DateTime(2023, 4, 2),
                      Details = "I am extremely disappointed with the product I received. It arrived late and damaged. Customer service has been unresponsive to my inquiries. This experience has left me frustrated and dissatisfied.",
                  },
                  new Feedback
                  {
                      Name = "Sally Joe",
                      Email = "SallyJoe@gmail.com",
                      Subject = "Misspelled name on page",
                      SpecimenID = 1,
                      CreateDate = new DateTime(2024, 2, 11),
                      Details = "I am extremely disappointed with the product I received. It arrived late and damaged. Customer service has been unresponsive to my inquiries. This experience has left me frustrated and dissatisfied.",
                  },
                  new Feedback
                  {
                      Name = "Billy Bob",
                      Email = "BillyBob@gmail.com",
                      Subject = "Specimen page is missing an image",
                      SpecimenID = 5,
                      CreateDate = new DateTime(2024, 2, 13),
                      Details = "I am writing to express my extreme dissatisfaction with the recent purchase I made from your website. Firstly, the product arrived significantly later than the promised delivery date, causing inconvenience and disruption to my plans. Upon opening the package, I was dismayed to discover that the item was damaged. The packaging seemed inadequate to protect the product during transit, as there were visible signs of mishandling and rough treatment. I immediately reached out to your customer service department to report the issue and request a resolution. However, I have been met with nothing but frustration and disappointment. Despite multiple attempts to contact your support team via phone and email, I have received no response. This lack of communication is unacceptable and reflects poorly on your company's commitment to customer satisfaction. As a loyal customer, I expected better from your brand. This experience has not only tarnished my perception of your company but has also caused me undue stress and inconvenience. I urge you to take immediate action to rectify this situation by providing a refund or replacement for the damaged product, as well as improving your customer service processes to prevent similar issues in the future. I sincerely hope that you will address my concerns promptly and restore my faith in your brand. Otherwise, I will have no choice but to take my business elsewhere and share my negative experience with others. Thank you for your attention to this matter."
                  }
 
                  );
            context.SaveChanges();

        }

    }
}
