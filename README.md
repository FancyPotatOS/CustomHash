 **CustomHash**

 My own version of a HashMap

.

 Note: I don't know if this is a unique idea, probably not, but I made it from my own self

 What separates this HashMap from others is that it is expandable but saves a decent amount of space.

 Let's assume all numbers are in base tableSize. For example, if your tableSize is 16, then all numbers is hexadecimal.

 You can know exactly what collisions will occur by looking at the number's digits. 
 For example, if you insert the keys 0xABCD and 0xEECD then there will be two collisions: D, then C.
 
 This forms a weird problem when calculating Big-O complexity, as technically the amount of steps required to make it to any case is how many digits the 
 two numbers share from the left to right in a row. For hexadecimal numbers and regular integers as for keys, the maximum collisions is 8!
 This is just log(**base 16**)(Integer.MaxValue).

 Does that make the Big-O complexity O(1)? I will challenge everybody who thinks not to a duel in the parking lot out back, up to you.

 How it functions is by creating an array of objects that is tableSize big. The insertion steps proceed as follows:
 1. Generate hash: modulus the key by tableSize
 2. Check position at hash in table
 3. If it is null, generate a Key class that stores actual key and value given
 4. If it is a Key class, extract Key class, edit key to remove the first digits, create a CustomHash in the position, then insert both into the table. This is recursive.
 5. If it is a CustomHash class, edit key to remove the first digits, then insert both into the table. This is recursive.

 The idea is simple: Store in a table, or if there is a collision, just create an array there and place both inside.

 Now, you might be worried about if there is a lot of collisions, but do not fret: collisions are unlikely. Since they are based off of the digits in base tableSize,
 it would seem to be as random as they get. If you are anticipating those types of patterns, then just hash the keys before you insert. Random chance in on your side!
 Even designing the worst-case sizes for the least amount of keys seems like a stretch. Lets say you have a table size of 10, and store using integers. Then the worst case
 would be inserting 0000000000 and 1000000000, then inserting 0000000001 and 1000000001, and so on. The number of collisions for every pair is 10, and the size is 10x the amount of keys necessary. This situation seems quite likely, 
 doesn't it? But, if you were to change the tableSize to 11, then there would only be one collision for every pair. The table size would be about 8x the amount necessary. If you were to insert randomly, 
 say by hashing the key first, then a chance of a collision of size n would be (1/tableSize)^n. This a stupidly small number for keys of significant sizes.

 Let's also not forget how efficient how inserting sequences in for 0 -> n. The amount of space wasted is at maximum (tableSize - 1).

 Hopefully this gives you an idea of why I wish to explore the potential in this system.