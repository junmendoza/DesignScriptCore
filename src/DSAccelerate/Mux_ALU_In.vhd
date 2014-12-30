----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    17:07:56 12/23/2014 
-- Design Name: 
-- Module Name:    Mux31_ALU_In - Behavioral 
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

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx primitives in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity Mux31_ALU_In is
	Port( 
			reset : in STD_LOGIC;
			select_index : in STD_LOGIC_VECTOR (7 downto 0);
			
			-- Inputs to ALU op1
			op11 : in STD_LOGIC_VECTOR (31 downto 0);
			op21 : in STD_LOGIC_VECTOR (31 downto 0);
			op31 : in STD_LOGIC_VECTOR (31 downto 0) := (others => '0');
			
			-- Inputs to ALU op2
			op12 : in STD_LOGIC_VECTOR (31 downto 0);
			op22 : in STD_LOGIC_VECTOR (31 downto 0);
			op32 : in STD_LOGIC_VECTOR (31 downto 0) := (others => '0');
			
			-- ALU inputs
			op1 : out STD_LOGIC_VECTOR (31 downto 0);
			op2 : out STD_LOGIC_VECTOR (31 downto 0)
		 );
end Mux31_ALU_In;

architecture Behavioral of Mux31_ALU_In is

begin
	process(reset, select_index)
	begin
		ResetSync : if reset = '0' then
			if select_index = X"00" then
				op1 <= op11;
				op2 <= op12;
			elsif select_index = X"01" then
				op1 <= op21;
				op2 <= op22;
			elsif select_index = X"02" then
				op1 <= op31;
				op2 <= op32;
			end if;
		end if ResetSync;
	end process;
end Behavioral;

