----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    13:58:05 12/10/2014 
-- Design Name: 
-- Module Name:    DecodeDisplayString - Behavioral 
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
use work.Definitions.all;

entity DecodeDisplayString is
	Port( 
			reset : in STD_LOGIC;
			exec_state : in EXECUTION_STATE;
			var_index : in STD_LOGIC_VECTOR (2 downto 0);
			char_array : out STD_LOGIC_VECTOR (79 downto 0)
		 );
end DecodeDisplayString;

architecture Behavioral of DecodeDisplayString is

--	constant c1 : STD_LOGIC_VECTOR(7 downto 0) := "01001010";
--	constant c2 : STD_LOGIC_VECTOR(7 downto 0) := "01110101";
--	constant c3 : STD_LOGIC_VECTOR(7 downto 0) := "01101110";
--	constant c4 : STD_LOGIC_VECTOR(7 downto 0) := "01001101";
--	constant c5 : STD_LOGIC_VECTOR(7 downto 0) := "01001010";
--	constant c6 : STD_LOGIC_VECTOR(7 downto 0) := "01110101";
--	constant c7 : STD_LOGIC_VECTOR(7 downto 0) := "01001010";
--	constant c8 : STD_LOGIC_VECTOR(7 downto 0) := "01001010";
--	constant c9 : STD_LOGIC_VECTOR(7 downto 0) := "01001010";
--	constant c10 : STD_LOGIC_VECTOR(7 downto 0) := "01001010";
	
begin

	ProcDecodeString : process (exec_state, var_index)
	begin
		ResetSync : if reset = '1' then
			char_array(79 downto 72) <= "01000001"; -- A 01000001
			char_array(71 downto 64) <= "01000001";
			char_array(63 downto 56) <= "01000001";
			char_array(55 downto 48) <= "01000001";
			char_array(47 downto 40) <= "01000001";
			char_array(39 downto 32) <= "01000001";
			char_array(31 downto 24) <= "01000001";
			char_array(23 downto 16) <= "01000001";
			char_array(15 downto 8)  <= "01000001";
			char_array(7 downto 0)   <= "01000001";
		elsif reset = '0' then
			IsExecutionComplete : if exec_state = EXEC_STATE_DONE then
				GetDisplayString : if var_index = "000" then
					char_array(79 downto 72) <= "01000010";	-- B 01000010
					char_array(71 downto 64) <= "01000010";
					char_array(63 downto 56) <= "01000010";
					char_array(55 downto 48) <= "01000010";
					char_array(47 downto 40) <= "01000010";
					char_array(39 downto 32) <= "01000010";
					char_array(31 downto 24) <= "01000010";
					char_array(23 downto 16) <= "01000010";
					char_array(15 downto 8)  <= "01000010";
					char_array(7 downto 0)   <= "01000010";
				elsif var_index = "001" then
					char_array(79 downto 72) <= "01000011";	-- C 01000011
					char_array(71 downto 64) <= "01000011";
					char_array(63 downto 56) <= "01000011";
					char_array(55 downto 48) <= "01000011";
					char_array(47 downto 40) <= "01000011";
					char_array(39 downto 32) <= "01000011";
					char_array(31 downto 24) <= "01000011";
					char_array(23 downto 16) <= "01000011";
					char_array(15 downto 8)  <= "01000011";
					char_array(7 downto 0)   <= "01000011";
				elsif var_index = "010" then
					char_array(79 downto 72) <= "01000100";	-- D 01000100
					char_array(71 downto 64) <= "01000100";
					char_array(63 downto 56) <= "01000100";
					char_array(55 downto 48) <= "01000100";
					char_array(47 downto 40) <= "01000100";
					char_array(39 downto 32) <= "01000100";
					char_array(31 downto 24) <= "01000100";
					char_array(23 downto 16) <= "01000100";
					char_array(15 downto 8)  <= "01000100";
					char_array(7 downto 0)   <= "01000100";
				end if GetDisplayString;
			end if IsExecutionComplete;
		end if ResetSync;
	end process ProcDecodeString;

end Behavioral;

