----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    22:04:43 01/29/2015 
-- Design Name: 
-- Module Name:    UART_Transmitter - Behavioral 
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

use IEEE.NUMERIC_STD.ALL;

library UNISIM;
use UNISIM.VComponents.all;


entity UART_Transmitter is
	Port( clock : in STD_LOGIC;
			reset : in STD_LOGIC;
			send_data : in STD_LOGIC;
			--data : in STD_LOGIC_VECTOR(7 downto 0);
			dataout : out STD_LOGIC
		 );
end UART_Transmitter;

architecture Behavioral of UART_Transmitter is

	type TRANSMIT_STATE is(	
				STATE_IDLE,
				STATE_START,
				STATE_TRANSMIT,
				STATE_DONE
			 );	
			 
	signal sent : STD_LOGIC;
	signal transmitState : TRANSMIT_STATE;
	signal data : std_logic_vector(7 downto 0) := "11001100";
	
begin
	process(clock, reset)
		variable bitnum : integer;  
		
	begin
		ResetSync : if reset = '1' then
			transmitState <= STATE_START;
			bitnum := 0;
			sent <= '0';
		elsif reset = '0' then
			if send_data = '1' then
				if sent = '0' then
					if transmitState = STATE_IDLE then
						transmitState <= STATE_START;
					elsif transmitState = STATE_START then
						dataout <= '0'; --start bit
						transmitState <= STATE_TRANSMIT;
					elsif transmitState = STATE_TRANSMIT then
						dataout <= data(bitnum); -- send data
						if bitnum = 8 then
							transmitState <= STATE_DONE;
						else
							bitnum := bitnum + 1;
						end if;
					elsif transmitState = STATE_DONE then	
						dataout <= '1'; -- stop bit
						sent <= '1';
					end if;
				end if;
			end if;
		end if ResetSync;
	end process;
	
end Behavioral;

