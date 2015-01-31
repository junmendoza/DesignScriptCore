----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    13:50:30 01/31/2015 
-- Design Name: 
-- Module Name:    BaudRateGenerator - Behavioral 
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

entity BaudRateGenerator is
	Port( 
			clock : in STD_LOGIC;
			reset : in STD_LOGIC;
			baudRateEnable : out STD_LOGIC
		 );
end BaudRateGenerator;

architecture Behavioral of BaudRateGenerator is

begin
	process(clock, reset)
		variable clock_ticks : integer;
		variable baud_rate : integer := 434;
	begin
		ResetSync: if reset = '1' then
			clock_ticks := 0;
			baudRateEnable <= '0';
		elsif reset = '0' then
			ClockSync : if rising_edge(clock) then
				baudRateEnable <= '0';
				if clock_ticks = baud_rate then
					baudRateEnable <= '1';
					clock_ticks := 0;
				else
					clock_ticks := clock_ticks + 1;
				end if;
			end if ClockSync;
		end if ResetSync;
	end process;

end Behavioral;

