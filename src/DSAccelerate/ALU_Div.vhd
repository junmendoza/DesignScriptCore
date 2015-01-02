----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    13:49:29 01/02/2015 
-- Design Name: 
-- Module Name:    ALU_Div - Behavioral 
-- Project Name: 
-- Target Devices: 
-- Tool versions: 
-- Description: 
--
-- Dependencies: 
--
-- Revision: 
-- Revision 0.01 - File Created
-- Additional Comments: 
--
----------------------------------------------------------------------------------
library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity ALU_Div is
    Port ( reset : in  STD_LOGIC;
           op1 : in  STD_LOGIC_VECTOR (31 downto 0);
           op2 : in  STD_LOGIC_VECTOR (31 downto 0);
           result : out  STD_LOGIC_VECTOR (31 downto 0));
end ALU_Div;

architecture Behavioral of ALU_Div is

begin

	process(op1, op2)
		
	begin
		if reset = '0' then			
			-- Todo
			-- Implement div
			result <= X"00000000";
		end if;
	end process;
end Behavioral;

