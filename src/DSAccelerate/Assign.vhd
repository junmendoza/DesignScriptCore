library IEEE;

use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity Assign is
port( 
	reset : in std_logic;
	a : in std_logic_vector(31 downto 0);
	return_Assign : out std_logic_vector(31 downto 0)
);
end Assign;

architecture Behavioral of Assign is

signal p : std_logic_vector(31 downto 0);
signal tSSA_0_969ac5ccb9494486bb8d04eb98b96c6e : std_logic_vector(31 downto 0);


begin

proc_1_Assign : process(a)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
p <= X"00000064";
end if ResetSync;


end process proc_1_Assign;

proc_2_p : process(p)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
tSSA_0_969ac5ccb9494486bb8d04eb98b96c6e <= p;
end if ResetSync;


end process proc_2_p;

proc_3_tSSA_0_969ac5ccb9494486bb8d04eb98b96c6e : process(tSSA_0_969ac5ccb9494486bb8d04eb98b96c6e)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
return_Assign <= tSSA_0_969ac5ccb9494486bb8d04eb98b96c6e;
end if ResetSync;


end process proc_3_tSSA_0_969ac5ccb9494486bb8d04eb98b96c6e;


end Behavioral;
