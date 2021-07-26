# SeradexTool
For use by Lauretano Sign Group LLC. by Christopher Gergler 
Copyright Christopher Gergler 2020-2021
Total Development Time - 2 months

## The Story of Development - A Letter from Chris
This project was developed for access to information stored in a soon to be decommissioned database that was originally established to use Seradex ERP for. Database schemas, indexes,and relation charts were not saved or available for use. I had to go into this blind.

Now the real kicker, I was mortified of making any changes to anything. No schema updates, no mapping of the tables... This database was still live during development, and I was only employed at LSG for a few months. It was a guillotine over my head. If ANYTHING went wrong with that data, I was done. 

We had this database on our server (I did not design this for flexibility, I had a singular purpose in mind) so unless accessing another Seradex database, this utility will likely not be useful. Database connections were hard-coded to prevent any accidental alterations during runtime or accidental cfg file adjustments. I had at one point entertained the idea of using a config for database connections, so that should we want to move the info onto a new server location we could quickly and easily adjust without the need for repackaging. That was squashed when the adjustment workload for Epicor 10 came in. Between development of this piece of software, and the overwhelming adjustments we needed to make to Epicor 10, it was easier just to write the connection into the initial script and repackage later if needed. *Luckily, this didn't bite me in the behind.*

#### Discovery
Cracking open this database was intense. I was using a system with 810 base tables with somewhat undecipherable naming conventions. Mind you, during development I was only 4 months or so into the industry? So even if I could crack open the tables, there was some information that I personally was lacking from the get go. 

We're talking names like:
ItemSpec                  - Okay, makes sense...  Item Specifications.
itemSpecFull              - Alright, more details right? Why is it smaller than ItemSpec?
ItemSpecOps               - Operations for Item Specifications I think?
ItemSpecProps             - Wait, isn't this just the other two tables?
ItemSpecFullStruc         - And this guy has no connection to the Item Spec table!?

So I did what any curious mind would do. I cracked open each table and dug through the columns to see what I was looking at. This just made things more confusing at first.

#### A Lesson in Naming Conventions

I didn't take the time to open up every single table. That would have cracked my noggin. Instead, I grabbed the absolute bare minimum tables and opened them up, and followed the daisy chain of relations. Again, I was terrified of generating anything, maps or relation charts, based off of this info in fear of accidentally clicking through something important like an index generation.

I'm sure there was a better way to do this, but I was already curious as to how wild this setup was. almost 20% of these columns were userdefined 250 digit chars. 
Another 20% were keys to other tables with decent naming conventions, but plenty were sensible SEEMING until the data in the column showed empty or 0. Almost everything was unused. 

In hindsight, it would have been easier to use a script to count the number of unique values in a column in order to rule out its use. Of course, with a literal 6 week deadline to create a utility from scratch, did I think of this? Of course not.
Eventually, I created my own paper chart that was plastered up around my cubicle. I wish I had taken photos of it, because it was hilarious to see these 1980's mauve sound barriers peppered with notebook pages. The few I did take are now lost to the ether as I changed phones without backing them up. 

#### Putting together the Algorithm

With the most loose understanding of how these tables connected, I began the process of putting together the program.
I needed to block out visual information so that the rest of the non-tech-savvy office members could use this without needing to change the way they thought. I had to match the currently existing system as closely as possible.

Annotated screenshots of the existing customizations laid out the framework for how everything would work.
A basic step by step outline of the process of getting data was written down over them.
 - Quotes, Open these tables: Estimates, EstimateDetails, Employees, Customers, etc...
 - Columns: EstimateNumber, CustomerName, ShippingLocation, etc...
 - Filters: Sort by column, search by content, build for speed (HAH! It's not fast, but it's faster than the original software).

Then I did this for each set of pages. Screen-shot of the needed info, written list of info pieces.
At the time, I only needed to jump into the Estimates, so I thought a singular page would work fine for this. Double click the row to open the Estimate so you can look at two at a time or compare different sections of very large estimates. *This, however, DID bite me in the behind*.

After I build half of the first page, I was told about the other pages needed. Estimates was a start, but I needed Sales Orders, Purchase Orders, and Invoices too.
While most of this could have been built into a separate file like my Toolkit.cs, I didn't migrate them.

#### Finally opening Visual Studio

Once I had an idea for how to build it out, the rest of the process was simple. Identify what needed to be done, build a function, encapsulate what needs to be protected info and send it over to my buddy in estimating who was terrified of breaking the software so that he could break it.
My exception codes were 90% jokes, but I had a keen understanding of what could have gone wrong. The bug that took me the longest to squash as an issue when double-clicking scroll bars. 

The initial algorithm included using a search button that would apply the text in the search bar and filter based on the selected column from a dropdown menu. This was a modular addition where I could easily add in more columns to filter by by linking an object in XML to a column. While the modular additions stayed, the button did not.
The button broke so frequently once I launched, failing to fire the filter or considering a double-click and opening up an unwanted window. One bug I still didn't figure out was sometimes the button would allow clickthrough to whatever was behind it. This property was never applied anywhere else, and I had to forcible disable clickthrough on JUST the button.

The button was instead replaced with a key-up event. Turned out that even if this slowed some of the searches down to a 0.5s delay, it was still way faster than the actual button-press search time for seradex in the configuration we used. Searching for an estimate was almost instant, and sped up as the filter text become more precise. Seradex, however was a singular query which simply took forever. 
If I had to establish as many dashboard connections, I'm sure my own method would have been just as slow, but for what we needed it made the office happy to have a quicker utility to work with. 

The other challenge I faced was accessing the file storage.
One function we had created was an automated folder creation system that allowed us to store project documents based on the Estimate Number. This was accessible in the Sales Orders and the Estimates, and became my biggest headache. The drive in question was a networked drive. Depending on whose computer the utility was running on, it would accept or reject the filepath. Tweaks and adjustments and more testing. Sometimes a lettered drive filepath worked. Sometimes a network drive filepath worked. Sometimes neither worked. By the time 1-0-4 was released, I had found filepaths built into the database and pulled them. However since some of them weren't filled in, that was the source of the error. It was rare, and became an outlier. Some instances had deleted folders, some had renamed folders. 

I deployed a total of 4 iterations, starting with Estimates, expanding to Sales orders, and eventually reaching Purchase Orders and Invoices.

Update as of 7-26-2021: The utility has been running strong with little need for updates. Occassionaly changing column orders for Invoicing is needed. The software is still used without need for update.

#### Post Mortem-ish
While the software may still need some adjustment, I consider its development complete. The utility works as needed, runs smoothly, and doesn't have any major bugs. It's MORE stable than the platform we're currently using, and should I want to use this as a starting point for creating an ERP system, I genuinely think it would be a fine adjustment.
it would likely work best for small to medium businesses, as it front-loads a large volume on load. It pulls a grid from the query and manipulates that on the front end. Since it doesn't ever apply that information, I left Cells with a writeable status. Only visual information could get changed so no harm, no foul.  

Plenty of utilities could have been migrated to a ViewModel file, like my Toolkit.cs. The GoToFolder functions could have been modular with input variables that allowed an expandability. Likewise, my open-other functions (OpenSalesorder) could have been established in there as well and passed to other pages.

This was a fun challenge though.
---
## Branch naming Conventions

#### Master
The master copy

#### Release X.X.X
When the software is packaged and sent out

#### X.X.X New Feature: ABC
When a new feature is added, and what the feature is.

#### X.X.X Debug: ABC
What feature is being debugged

#### X.X.X Cleanup: ABC
Refactoring the scripts to be cleaner and clearer. See Wikipedia for Code Refactoring Definition
Eliminating unused variables or unaccessed functions. Overall making it more readable and more maintainable.

---

