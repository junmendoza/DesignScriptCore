library IEEE;

use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity Mux_31_Component_In is
port( 
	reset : in std_logic;
	select_index : in std_logic_vector(7 downto 0);
	op11 : in std_logic_vector(31 downto 0);
	op21 : in std_logic_vector(31 downto 0);
	op31 : in std_logic_vector(31 downto 0);
	op12 : in std_logic_vector(31 downto 0);
	op22 : in std_logic_vector(31 downto 0);
	op32 : in std_logic_vector(31 downto 0);
	op1 : out std_logic_vector(31 downto 0);
	op2 : out std_logic_vector(31 downto 0)
);
end Mux_31_Component_In;

architecture Behavioral of Mux_31_Component_In is



begin

proc_1_Mux_31_Component_In : process(reset, select_index)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
if select_index = X"00" then
op1 <= op11;
op2 <= op12;

elsif select_index = X"01" then
op1 <= op21;
op2 <= op22;

elsif select_index = X"02" then
op1 <= op31;
op2 <= op32;

end if ;

end if ResetSync;


end process proc_1_Mux_31_Component_In;


end Behavioral;
